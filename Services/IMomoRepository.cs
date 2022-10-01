using PetStoreApi.Domain;
using PetStoreApi.DTO.MomoDTO;

namespace PetStoreApi.Services
{
    public interface IMomoRepository
    {
        Task<AppServiceResult<MomoResponse>> CreatePaymentMomo(double amount, string notifyUrl, string returnUrl);
    }
}
