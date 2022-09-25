using Microsoft.AspNetCore.Mvc;
using PetStoreApi.Domain;
using PetStoreApi.DTO.UserDTO;
using PetStoreApi.Services;

namespace PetStoreApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAppUserRepository _appUserRepository;

        public UserController(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
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
    }
}
