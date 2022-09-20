using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.BreedDTO;

namespace PetStoreApi.Services
{
    public interface ICategoryRepository
    {
        Category GetCategory(int? id);
    }
}
