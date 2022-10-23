using Microsoft.EntityFrameworkCore;
using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.OriginDTO;
using PetStoreApi.Helpers;

namespace PetStoreApi.Services.Repositories
{
    public class OriginRepository : GenericRepository<Origin>, IOriginRepository
    {
        private readonly DataContext _context;

        public OriginRepository(DataContext context) : base(context)
        {
        }

        public void CreateOrigin(Origin origin)
        {
            Create(origin);
        }

        public void DeleteOrigin(Origin origin)
        {
            Delete(origin);
        }

        public async Task<IEnumerable<Origin>> GetAllOriginsAsync()
        {
            return await FindAll().OrderBy(b => b.Name).ToListAsync();
        }

        public void UpdateOrigin(Origin origin)
        {
            Update(origin);
        }

        public async Task<Origin> GetOrigin(int? id)
        {
            return await FindByCondition(b => b.Id.Equals(id)).FirstOrDefaultAsync();
        }
    }
}
