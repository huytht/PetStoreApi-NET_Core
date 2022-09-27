using AutoMapper;
using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using PetStoreApi.Constants;
using PetStoreApi.Data.Entity;
using PetStoreApi.Domain;
using PetStoreApi.DTO.UserDTO;
using PetStoreApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace PetStoreApi.Services.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<AppUserRepository> _logger;
        private readonly IMapper _mapper;
        private readonly IJwtProvider _jwtProvider;
        private readonly AppSetting _appSetting;
        private readonly IEmailSender _emailSender;
        public AppUserRepository(DataContext context, ILogger<AppUserRepository> logger, IMapper mapper, IJwtProvider jwtProvider, IOptionsMonitor<AppSetting> optionsMonitor, IEmailSender emailSender)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _appSetting = optionsMonitor.CurrentValue;
            _emailSender = emailSender;
        }

        public async Task<AppServiceResult<UserLoginResponseDto>> Login(UserLoginDto userLogin)
        {
            string hashPassword = BC.HashPassword(userLogin.Password);
            
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Username == userLogin.Username);
            
            if (user == null && !hashPassword.Equals(user?.Password))
            {
                return new AppServiceResult<UserLoginResponseDto>(false, 404, "Tài khoản hoặc mật khẩu không chính xác. Vui lòng thử lại.", null);
            } else
            {
                if (!user.Enabled)
                {
                    return new AppServiceResult<UserLoginResponseDto>(false, 0, "Tài khoản của bạn chưa được kích hoạt. Vui lòng xác nhận email hoặc liên hệ với quản trị viên.", null);
                }
                if (!user.AccountNonLocked)
                {
                    return new AppServiceResult<UserLoginResponseDto>(false, 0, "Tài khoản của bạn đã bị khóa. Vui lòng liên hệ với quản trị viên", null);
                }
            }
           

            UserLoginResponseDto result = _mapper.Map<UserLoginResponseDto>(user);
            
            var token = await _jwtProvider.GenerateToken(user);
            var userInfo = await _context.UserInfos.FirstOrDefaultAsync(u => u.AppUser.Id == result.Id);

            result.AvatarImg = userInfo.AvatarImg;
            result.Token = token;

            return new AppServiceResult<UserLoginResponseDto>(true, 0, "Login successful", result);

        }

        public async Task<AppBaseResult> Register(UserRegisterDto userRegister)
        {
            try
            {
                var userByEmail = await _context.AppUsers.FirstOrDefaultAsync(u => u.Email.Equals(userRegister.Email));
                if (userByEmail != null)
                {
                    _logger.LogWarning("Email is exist: " + userRegister.Email + ", Cannot further process!");

                    return AppBaseResult.GenarateIsFailed(101,
                        "Email " + userRegister.Email + " đã được sử dụng. Vui lòng nhập email khác");
                }
                var userByUsername = await _context.AppUsers.FirstOrDefaultAsync(u => u.Username.Equals(userRegister.Username));
                if (userByUsername != null)
                {
                    _logger.LogWarning("Username is exist: " + userRegister.Username + ", Cannot further process!");

                    return AppBaseResult.GenarateIsFailed(101,
                        "Tên tài khoản " + userRegister.Username + " đã tồn tại.");
                }
                AppUser user = _mapper.Map<AppUser>(userRegister);
                user.Id = new Guid();
                var defaultRole = await _context.AppRoles.FirstOrDefaultAsync(r => r.Name.Equals(RoleConstant.ROLE_MEMBER));
                user.AppUserRoles.Add(new AppUserRole(user.Id, defaultRole.Id));

                user.UserInfo = new UserInfo();

                user.UserInfo.AvatarImg = FileConstant.TEMP_PROFILE_IMAGE_BASE_URL + userRegister.Username;

                user.Enabled = false;
                user.AccountNonLocked = true;

                user.Password = BC.HashPassword(userRegister.Password);

                await _context.AppUsers.AddAsync(user);
                await _context.SaveChangesAsync();

                //string toEmailFormat = user.Email + " <" + user.Email + ">";
                Message message = new Message(user.Email, "Send Email Verify", null, null, user);

                await _emailSender.SendEmailAsync(message);

                return AppBaseResult.GenarateIsSucceed();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return AppBaseResult.GenarateIsFailed(99, "Unknown");
            }
        }
        public async Task<AppServiceResult<TokenModel>> RenewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSetting.SecretKey);
            var tokenValidateParam = new TokenValidationParameters
            {
                //tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                //ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false //ko kiểm tra token hết hạn
            };
            try
            {
                //check 1: AccessToken valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.AccessToken, tokenValidateParam, out var validatedToken);

                //check 2: Check alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)//false
                    {
                        return new AppServiceResult<TokenModel>(false, 0, "Invalid Token", null);
                    }
                }

                //check 3: Check accessToken expire?
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if (expireDate > DateTime.UtcNow)
                {
                    return new AppServiceResult<TokenModel>(false, 0, "Access token has not yet expired", null);
                }

                //check 4: Check refreshtoken exist in DB
                var storedToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);
                if (storedToken == null)
                {
                    return new AppServiceResult<TokenModel>(false, 0, "Refresh token does not exist", null);
                }

                //check 5: check refreshToken is used/revoked?
                if (storedToken.IsUsed)
                {
                    return new AppServiceResult<TokenModel>(false, 0, "Refresh token has been used", null);
                }
                if (storedToken.IsRevoked)
                {
                    return new AppServiceResult<TokenModel>(false, 0, "Refresh token has been revoked", null);
                }

                //check 6: AccessToken id == JwtId in RefreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                {
                    return new AppServiceResult<TokenModel>(false, 0, "Token doesn't match", null);
                }

                //Update token is used
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                _context.Update(storedToken);
                await _context.SaveChangesAsync();

                //create new token
                var user = await _context.AppUsers.SingleOrDefaultAsync(nd => nd.Id == storedToken.UserId);
                var token = await _jwtProvider.GenerateToken(user);

                return new AppServiceResult<TokenModel>(true, 0, "Renew token success", token);
            }
            catch (Exception ex)
            {
                return new AppServiceResult<TokenModel>(false, 0, "Something went wrong", null);
            }
        }

        public async Task<AppBaseResult> VerifyEmail(string token)
        {
            VerificationToken vToken = await _context.VerificationTokens.FirstOrDefaultAsync(v => v.Token == token);

            if (vToken != null)
            {
                if (vToken.VerifyDate != null)
                {
                    _logger.LogWarning("Token verified");

                    return AppBaseResult.GenarateIsFailed(101, "Token verified!");
                }

                vToken.IsVerify = true;
                vToken.VerifyDate = DateTime.Now;

                var user = _context.AppUsers.SingleOrDefault(nd => nd.Id == vToken.UserId);
                user.Enabled = true;

                await _context.SaveChangesAsync();

                return AppBaseResult.GenarateIsSucceed();
            } else
            {
                _logger.LogWarning("Token is not exist: " + token);
                return AppBaseResult.GenarateIsFailed(99, "Token is not exist!");
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

            return dateTimeInterval;
        }
    }
}
