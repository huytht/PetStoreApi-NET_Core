using com.sun.xml.@internal.bind.v2.model.core;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PetStoreApi.Constants;
using PetStoreApi.Domain;
using PetStoreApi.DTO.OrderDTO;
using PetStoreApi.DTO.ResponseDTO;
using PetStoreApi.DTO.UserDTO;
using PetStoreApi.DTO.UserInfoDTO;
using PetStoreApi.Helpers;
using PetStoreApi.Services;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace PetStoreApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly Variable _options;

        public UserController(IAppUserRepository appUserRepository, IOrderRepository orderRepository, IWebHostEnvironment hostingEnvironment, IOptions<Variable> options, DataContext context)
        {
            _context = context;
            _appUserRepository = appUserRepository;
            _orderRepository = orderRepository;
            _hostingEnvironment = hostingEnvironment;
            _options = options.Value;
        }
        [HttpGet("export")]
        [Authorize(Roles = "ROLE_ADMIN")]
        public IActionResult exportToCSV()
        {
            var userList = _context.AppUsers.Include("UserInfo").ToList().Select(user =>
               new ReportCSVModel()
               {
                   UserId = user.Id,
                   Email = user.Email,
                   FullName = user.UserInfo.LastName + " " + user.UserInfo.FirstName 
               }
           );

            List<ReportCSVModel> reportCSVModel = userList.ToList();

            var stream = new MemoryStream();
            using (var writeFile = new StreamWriter(stream, leaveOpen: true))
            {
                var csv = new CsvWriter(writeFile, true);
                csv.WriteRecords(reportCSVModel);
            }
            stream.Position = 0; //reset stream
            return File(stream, "application/octet-stream", "Reports.csv");
        }
        [HttpGet("verify/{token}")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            AppBaseResult result = await _appUserRepository.VerifyEmail(token);

            string tempateFilePath = _hostingEnvironment.ContentRootPath + (result.success ? "/Templates/verify-success.html" : "/Templates/verify-failed.html");

            return Content(System.IO.File.ReadAllText(tempateFilePath).Replace("HOME_PAGE_CLIENT_URL", _options.HomePageClient), "text/html", Encoding.UTF8);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDto userLogin)
        {
            AppServiceResult<UserLoginResponseDto> result = await _appUserRepository.Login(userLogin);

            return result.success ? Ok(new HttpResponseSuccess<UserLoginResponseDto>(result.data)) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegister)
        {
            AppBaseResult result = await _appUserRepository.Register(userRegister);

            return result.success ? Ok(new HttpResponseSuccess<string>("Succeed!")) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel model)
        {
            AppServiceResult<TokenModel> result = await _appUserRepository.RenewToken(model);

            return result.success ? Ok(new HttpResponseSuccess<TokenModel>(result.data)) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpGet("order")]
        [Authorize(Roles = "ROLE_MEMBER, ROLE_ADMIN")]
        public async Task<IActionResult> GetOrderlist(int orderStatus, int pageNumber = PaginationConstant.PAGE_NUMBER_DEFAULT, int pageSize = PaginationConstant.PAGE_SIZE_DEFAULT)
        {
            PageParam pageParam = new PageParam(pageNumber, pageSize);

            AppServiceResult<PaginatedList<OrderDto>> result = await _orderRepository.GetListOrder(orderStatus, pageParam);

            return result.success ? Ok(new HttpResponseSuccess<PaginatedList<OrderDto>>(result.data)) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpGet("profiles")]
        [Authorize(Roles = "ROLE_MEMBER, ROLE_ADMIN")]
        public async Task<IActionResult> GetProfile(Guid userId)
        {
            AppServiceResult<UserInfoResponseDto> result = await _appUserRepository.GetProfile(userId);

            return result.success ? Ok(new HttpResponseSuccess<UserInfoResponseDto>(result.data)) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpPut("profiles")]
        [Authorize(Roles = "ROLE_MEMBER, ROLE_ADMIN")]
        public async Task<IActionResult> SaveProfile(UserInfoRequestDto userInfo)
        {
            var result = await _appUserRepository.SaveProfile(userInfo);

            return result.success ? Ok(new HttpResponseSuccess<string>("Succeed!")) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpPut("password")]
        [Authorize(Roles = "ROLE_MEMBER, ROLE_ADMIN")]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {
            var result = await _appUserRepository.ChangePassword(changePassword);

            return result.success ? Ok(new HttpResponseSuccess<string>("Succeed!")) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpPost("upload-profile-image")]
        [Authorize(Roles = "ROLE_MEMBER, ROLE_ADMIN")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var result = await _appUserRepository.UploadImage(file);

            return result.success ? Ok(new HttpResponseSuccess<string>(result.data)) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpPost("update-status")]
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task<IActionResult> UpdateStatus(UserStatusDto userStatus)
        {
            var result = await _appUserRepository.UpdateActive(userStatus);

            return result.success ? Ok(new HttpResponseSuccess<string>("Succeed!")) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpGet("list")]
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _appUserRepository.GetUserList();

            return result.success ? Ok(new HttpResponseSuccess<List<AppUserForAdminDto>>(result.data)) : BadRequest(new HttpResponseError(null, result.message));
        }
    }
}
