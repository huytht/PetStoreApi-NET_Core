using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.OriginDTO;

namespace PetStoreApi.Services
{
    public interface IOriginRepository
    {
        Task<Origin> GetOrigin(int? id);
        Task<IEnumerable<Origin>> GetAllOriginsAsync();
        void CreateOrigin(Origin origin);
        void UpdateOrigin(Origin origin);
        void DeleteOrigin(Origin origin);
    }
}
