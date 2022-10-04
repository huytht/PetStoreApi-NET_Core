using PetStoreApi.Domain;
using PetStoreApi.DTO.MomoDTO;

namespace PetStoreApi.Services
{
    public interface IMomoRepository
    {
        Task<AppServiceResult<object>> CreatePaymentMomo(long amount, string notifyUrl, string returnUrl);
    }
}
