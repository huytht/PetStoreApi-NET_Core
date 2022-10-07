using PetStoreApi.Data.Entity;
using PetStoreApi.Domain;
using PetStoreApi.DTO.UserDTO;
using PetStoreApi.DTO.UserInfoDTO;

namespace PetStoreApi.Services
{
    public interface IAppUserRepository
    {
        Task<AppServiceResult<UserLoginResponseDto>> Login(UserLoginDto userLogin);
        Task<AppBaseResult> Register(UserRegisterDto userRegister);
        Task<AppServiceResult<TokenModel>> RenewToken(TokenModel model);
        Task<AppBaseResult> VerifyEmail(string token);
        Task<AppServiceResult<List<AppUserForAdminDto>>> GetUserList();
        Task<AppServiceResult<UserInfoResponseDto>> GetProfile(Guid userId);
        Task<AppBaseResult> SaveProfile(UserInfoRequestDto userInfo);
        Task<AppBaseResult> ChangePassword(ChangePassword changePassword);
        Task<AppServiceResult<string>> UploadImage(IFormFile file);
        Task<AppBaseResult> UpdateActive(UserStatusDto userStatus);

    }
}
