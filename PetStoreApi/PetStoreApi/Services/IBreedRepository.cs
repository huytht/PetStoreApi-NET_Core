using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.BreedDTO;

namespace PetStoreApi.Services
{
    public interface IBreedRepository
    {
        Task<Breed> GetBreed(int? id);
    }
}
