using Microsoft.EntityFrameworkCore;
using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.BreedDTO;
using PetStoreApi.Helpers;

namespace PetStoreApi.Services.Repositories
{
    public class BreedRepository : IBreedRepository
    {
        private readonly DataContext _context;

        public BreedRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Breed> GetBreed(int? id)
        {
            try
            {
                var breed = await _context.Breeds.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
                if (breed != null)
                {
                    return new Breed
                    {
                        Id = breed.Id,
                        Name = breed.Name,
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            return null;
        }
    }
}
