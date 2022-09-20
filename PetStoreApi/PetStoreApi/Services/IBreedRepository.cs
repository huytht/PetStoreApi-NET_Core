using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.BreedDTO;

namespace PetStoreApi.Services
{
    public interface IBreedRepository
    {
        Breed GetBreed(int? id);
    }
}
