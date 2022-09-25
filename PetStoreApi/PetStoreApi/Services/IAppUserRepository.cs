using PetStoreApi.Domain;
using PetStoreApi.DTO.UserDTO;

namespace PetStoreApi.Services
{
    public interface IAppUserRepository
    {
        Task<AppServiceResult<UserLoginResponseDto>> Login(UserLoginDto userLogin);
        Task<AppBaseResult> Register(UserRegisterDto userRegister);
        Task<AppServiceResult<TokenModel>> RenewToken(TokenModel model);
        Task<AppBaseResult> VerifyEmail(string token);
    }
}
