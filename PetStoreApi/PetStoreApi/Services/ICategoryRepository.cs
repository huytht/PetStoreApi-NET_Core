using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.BreedDTO;

namespace PetStoreApi.Services
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategory(int? id);
    }
}
