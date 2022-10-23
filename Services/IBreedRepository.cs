using java.security.acl;
using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.BreedDTO;

namespace PetStoreApi.Services
{
    public interface IBreedRepository : IGenericRepository<Breed>
    {
        Task<Breed> GetBreed(int? id);
        Task<IEnumerable<Breed>> GetAllBreedsAsync();
        void CreateBreed(Breed breed);
        void UpdateBreed(Breed breed);
        void DeleteBreed(Breed breed);

    }
}
