using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetStoreApi.Constants;
using PetStoreApi.Domain;
using PetStoreApi.DTO.OrderDTO;
using PetStoreApi.DTO.UserDTO;
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

            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDto userLogin)
        {
            AppServiceResult<UserLoginResponseDto> result = await _appUserRepository.Login(userLogin);

            return Ok(result);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegister)
        {
            AppBaseResult result = await _appUserRepository.Register(userRegister);

            return Ok(result);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel model)
        {
            AppServiceResult<TokenModel> result = await _appUserRepository.RenewToken(model);

            return Ok(result);
        }
        [HttpGet("order")]
        [Authorize]
        public async Task<IActionResult> GetOrderlist(int orderStatus, int pageNumber = PaginationConstant.PAGE_NUMBER_DEFAULT, int pageSize = PaginationConstant.PAGE_SIZE_DEFAULT)
        {
            PageParam pageParam = new PageParam(pageNumber, pageSize);

            AppServiceResult<PaginatedList<OrderDto>> result = await _orderRepository.GetListOrder(orderStatus, pageParam);

            return Ok(result);
        }
    }
}
