using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetStoreApi.Constants;
using PetStoreApi.Domain;
using PetStoreApi.DTO.OrderDTO;
using PetStoreApi.DTO.UserDTO;
using PetStoreApi.DTO.UserInfoDTO;
using PetStoreApi.Services;
using System.Security.Claims;

namespace PetStoreApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IOrderRepository _orderRepository;

        public UserController(IAppUserRepository appUserRepository, IOrderRepository orderRepository)
        {
            _appUserRepository = appUserRepository;
            _orderRepository = orderRepository;
        }
        [HttpGet("verify/{token}")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            AppBaseResult result = await _appUserRepository.VerifyEmail(token);

            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDto userLogin)
        {
            AppServiceResult<UserLoginResponseDto> result = await _appUserRepository.Login(userLogin);

            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegister)
        {
            AppBaseResult result = await _appUserRepository.Register(userRegister);

            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel model)
        {
            AppServiceResult<TokenModel> result = await _appUserRepository.RenewToken(model);

            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("order")]
        [Authorize]
        public async Task<IActionResult> GetOrderlist(int orderStatus, int pageNumber = PaginationConstant.PAGE_NUMBER_DEFAULT, int pageSize = PaginationConstant.PAGE_SIZE_DEFAULT)
        {
            PageParam pageParam = new PageParam(pageNumber, pageSize);

            AppServiceResult<PaginatedList<OrderDto>> result = await _orderRepository.GetListOrder(orderStatus, pageParam);

            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("profiles")]
        [Authorize]
        public async Task<IActionResult> GetProfile(Guid userId)
        {
            AppServiceResult<UserInfoResponseDto> result = await _appUserRepository.GetProfile(userId);

            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpPut("profiles")]
        [Authorize]
        public async Task<IActionResult> SaveProfile(UserInfoRequestDto userInfo)
        {
            var result = await _appUserRepository.SaveProfile(userInfo);

            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpPut("password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {
            var result = await _appUserRepository.ChangePassword(changePassword);

            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("upload-profile-image")]
        [Authorize]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var result = await _appUserRepository.UploadImage(file);

            return result.success ? Ok(result) : BadRequest(result);
        }
    }
}
