using PetStoreApi.Domain;
using PetStoreApi.DTO.PurchaseDTO;

namespace PetStoreApi.Services
{
    public interface ICheckoutRepository
    {
        Task<AppServiceResult<PurchaseResponse>> PlaceOrder(PurchaseDto purchase);
    }
}
