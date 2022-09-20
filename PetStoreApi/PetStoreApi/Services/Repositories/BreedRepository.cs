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

        public Breed GetBreed(int? id)
        {
            try
            {
                Breed? breed = _context.Breeds.AsNoTracking().FirstOrDefault(b => b.Id == id);
                Console.WriteLine("===========================>" + breed.ToString());
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
                return null;
            }
            return null;
        }
    }
}
