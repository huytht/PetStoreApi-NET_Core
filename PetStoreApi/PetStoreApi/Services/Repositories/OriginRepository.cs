using Microsoft.EntityFrameworkCore;
using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.BreedDTO;
using PetStoreApi.Helpers;

namespace PetStoreApi.Services.Repositories
{
    public class OriginRepository : IOriginRepository
    {
        private readonly DataContext _context;

        public OriginRepository(DataContext context)
        {
            _context = context;
        }

        public Origin GetOrigin(int? id)
        {
            try
            {
                var origin = _context.Origins.AsNoTracking().FirstOrDefault(b => b.Id == id);
                if (origin != null)
                {
                    return new Origin
                    {
                        Id = origin.Id,
                        Name = origin.Name,
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
