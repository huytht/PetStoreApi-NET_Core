using PetStoreApi.Data.Entity;
using PetStoreApi.Domain;
using PetStoreApi.DTO.BreedDTO;
using PetStoreApi.DTO.CategoryDTO;
using PetStoreApi.DTO.OrderStatusDTO;
using PetStoreApi.DTO.OriginDTO;

namespace PetStoreApi.Services
{
    public interface ICommonRepository
    {
        Task<AppServiceResult<List<Breed?>>> GetAllBreedByCategory(int categoryId);

        Task<AppServiceResult<List<CategoryDto>>> GetCategoryList();

        Task<AppServiceResult<List<OrderStatusDto>>> GetOrderStatusList();

        Task<AppServiceResult<List<OriginDto>>> GetOriginList();

        Task<AppServiceResult<List<BreedDto>>> GetBreedList();
    }
}
