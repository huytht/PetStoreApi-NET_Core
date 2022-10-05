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
using PetStoreApi.DTO.UserInfoDTO;
using System.Security.Claims;

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
        private readonly IHttpContextAccessor? _httpContextAccessor;
        private readonly IFileRepository _fileRepository;

        public AppUserRepository(DataContext context, ILogger<AppUserRepository> logger, IMapper mapper, IJwtProvider jwtProvider, IOptionsMonitor<AppSetting> optionsMonitor, IEmailSender emailSender, IHttpContextAccessor? httpContextAccessor, IFileRepository fileRepository)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _appSetting = optionsMonitor.CurrentValue;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
            _fileRepository = fileRepository;
        }

        public async Task<AppBaseResult> ChangePassword(ChangePassword changePassword)
        {
            try
            {
                string currentUsername = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (!currentUsername.Equals(changePassword.Username))
                {
                    _logger.LogWarning("Not match UserId, Cannot further process!");

                    return AppBaseResult.GenarateIsFailed(101, "Not match UserId");
                }

                if (changePassword.NewPassword.Equals(changePassword.OldPassword))
                {
                    _logger.LogWarning("New password equals old password, Cannot further process!");

                    return AppBaseResult.GenarateIsFailed(101, "Mật khẩu mới trùng với mật khẩu cũ!");
                }

                var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Username.Equals(currentUsername));

                if (user == null)
                {
                    _logger.LogWarning("User is not exist!, Cannot further process!");

                    return AppBaseResult.GenarateIsFailed(101, "User is not exist!");
                }

                if (BC.Verify(changePassword.OldPassword, user.Password) == false)
                {
                    _logger.LogWarning("Password incorrect, Cannot further process!");

                    return AppBaseResult.GenarateIsFailed(101, "Sai mật khẩu!");
                }

                user.Password = BC.HashPassword(changePassword.NewPassword);

                await _context.SaveChangesAsync();

                return AppBaseResult.GenarateIsSucceed();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return AppBaseResult.GenarateIsFailed(99, "Unknown");
            }

        }

        public async Task<AppServiceResult<UserInfoResponseDto>> GetProfile(Guid userId)
        {
            try
            {
                UserInfoResponseDto userInfo = new UserInfoResponseDto();
                var user = await _context.AppUsers.Include("UserInfo").FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    _logger.LogWarning("AppUser is null, Cannot further process!");
                    return new AppServiceResult<UserInfoResponseDto>(false, 101,
                            "User is not exist!", null);
                }

                if (user.UserInfo != null)
                {
                    // TODO: Implement mapping
                    userInfo.UserId = user.Id;
                    userInfo.Email = user.Email;
                    userInfo.Username = user.Username;
                    userInfo.FirstName = user.UserInfo.FirstName;
                    userInfo.LastName = user.UserInfo.LastName;
                    userInfo.AvatarImg = user.UserInfo.AvatarImg;
                    userInfo.Phone = user.UserInfo.Phone;
                }

                return new AppServiceResult<UserInfoResponseDto>(true, 0, "Success", userInfo);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new AppServiceResult<UserInfoResponseDto>(false, 99, "Unknown", null);
            }
        }

        public async Task<AppServiceResult<List<AppUser>>> GetUserList()
        {
            try
            {
                var users = _context.AppUsers.Select(u => u);

                return new AppServiceResult<List<AppUser>>(true, 0, "Succeed!", users.ToList());
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new AppServiceResult<List<AppUser>>(false, 99, "Unknown", null);
            }
        }

        public async Task<AppServiceResult<UserLoginResponseDto>> Login(UserLoginDto userLogin)
        {
            try
            {
                var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Username == userLogin.Username);

                if (user == null || BC.Verify(userLogin.Password, user.Password) == false)
                {
                    return new AppServiceResult<UserLoginResponseDto>(false, 404, "Tài khoản hoặc mật khẩu không chính xác. Vui lòng thử lại.", null);
                }
                else
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
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new AppServiceResult<UserLoginResponseDto>(false, 99, "Unknown", null);
            }
           

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

                Message message = new Message(user.Email, "Send Email Verify", null, null, user);

                _emailSender.SendEmail(message);

                return AppBaseResult.GenarateIsSucceed();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.InnerException.Message);
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

        public async Task<AppBaseResult> SaveProfile(UserInfoRequestDto userInfo)
        {
            try
            {
                string currentUsername = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (currentUsername != null)
                {
                    var user = await _context.AppUsers.Include("UserInfo").FirstOrDefaultAsync(u => u.Username.Equals(currentUsername));

                    if (userInfo.UserId != user?.Id)
                    {
                        _logger.LogWarning("Not match UserId, Cannot further process!");

                        return AppBaseResult.GenarateIsFailed(101, "User is not match id");
                    }

                    // TODO: Implement mapping
                    var userByEmail = await _context.AppUsers.FirstOrDefaultAsync(u => u.Email.Equals(userInfo.Email));

                    if (userByEmail != null && userByEmail.Id != user.Id)
                    {
                        _logger.LogWarning("Email is exist: " + userInfo.Email + ", Cannot further process!");

                        return AppBaseResult.GenarateIsFailed(101, "Email " + userInfo.Email + " đã được sử dụng. Vui lòng nhập email khác");
                    }

                    user.Email = userInfo.Email;
                    user.UserInfo.FirstName = userInfo.FirstName;
                    user.UserInfo.LastName = userInfo.LastName;
                    user.UserInfo.Phone = userInfo.Phone;

                    // Save user
                    await _context.SaveChangesAsync();

                    return AppBaseResult.GenarateIsSucceed();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return AppBaseResult.GenarateIsFailed(99, "Unknown");
            }
            return AppBaseResult.GenarateIsFailed(99, "Unknown");
        }

        public async Task<AppServiceResult<string>> UploadImage(IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    string currentUsername = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                    if (currentUsername != null)
                    {

                        var user = await _context.AppUsers.Include("UserInfo").FirstOrDefaultAsync(u => u.Username.Equals(currentUsername));
                        if (user == null)
                        {
                            _logger.LogWarning("User is not exist, Cannot further process!");

                            return new AppServiceResult<string>(false, 101, " User is not exist", null);
                        }

                        string imagePath = _fileRepository.Upload(user.Username, file);
                        user.UserInfo.AvatarImg = imagePath;

                        await _context.SaveChangesAsync();

                        return new AppServiceResult<string>(true, 0, "Succeed!", imagePath);
                    }
                }
                else
                {
                    _logger.LogWarning("Image file is not null, Cannot further process!");

                    return new AppServiceResult<string>(false, 101, "Image file is not null", null);
                }
            }
            catch (IOException e)
            {
                _logger.LogError(e.Message);

                return new AppServiceResult<string>(false, 99, "Unknown", null);
            }
            return new AppServiceResult<string>(false, 99, "Unknown", null);
        }

        public async Task<AppBaseResult> VerifyEmail(string token)
        {
            VerificationToken vToken = await _context.VerificationTokens.Include("AppUser").FirstOrDefaultAsync(v => v.Token == token);

            if (vToken != null)
            {
                if (vToken.VerifyDate != null)
                {
                    _logger.LogWarning("Token verified");

                    return AppBaseResult.GenarateIsFailed(101, "Token verified!");
                }

                vToken.IsVerify = true;
                vToken.VerifyDate = DateTime.Now;
                vToken.AppUser.Enabled = true;

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
