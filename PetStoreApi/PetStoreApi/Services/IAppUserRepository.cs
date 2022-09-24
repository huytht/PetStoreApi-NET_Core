using PetStoreApi.Domain;
using PetStoreApi.DTO.UserDTO;

namespace PetStoreApi.Services
{
    public interface IAppUserRepository
    {
        Task<AppServiceResult<UserLoginResponseDto>> Login(UserLoginDto userLogin);
        AppBaseResult Register(UserRegisterDto userRegister);
        Task<AppServiceResult<TokenModel>> RenewToken(TokenModel model);
    }
}
