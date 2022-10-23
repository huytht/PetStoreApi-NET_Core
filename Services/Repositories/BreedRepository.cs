using java.security.acl;
using Microsoft.EntityFrameworkCore;
using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.BreedDTO;
using PetStoreApi.Helpers;

namespace PetStoreApi.Services.Repositories
{
    public class BreedRepository : GenericRepository<Breed>, IBreedRepository
    {
        public BreedRepository(DataContext context) : base(context)
        {
        }

        public void CreateBreed(Breed breed)
        {
            Create(breed);
        }

        public void DeleteBreed(Breed breed)
        {
            Delete(breed);
        }

        public async Task<IEnumerable<Breed>> GetAllBreedsAsync()
        {
            return await FindAll().OrderBy(b => b.Name).ToListAsync();
        }

        public void UpdateBreed(Breed breed)
        {
            Update(breed);
        }

        public async Task<Breed> GetBreed(int? id)
        {
            return await FindByCondition(b => b.Id.Equals(id)).FirstOrDefaultAsync();
        }
    }
}
