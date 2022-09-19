using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.BreedDTO;

namespace PetStoreApi.Services
{
    public interface IOriginRepository
    {
        Task<Origin> GetOrigin(int? id);
    }
}
