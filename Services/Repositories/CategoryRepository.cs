using Microsoft.EntityFrameworkCore;
using PetStoreApi.Data.Entity;
using PetStoreApi.Helpers;

namespace PetStoreApi.Services.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public Category GetCategory(int? id)
        {
            try
            {
                var category = _context.Categories.AsNoTracking().FirstOrDefault(c => c.Id == id);
                if (category != null)
                {
                    return new Category
                    {
                        Id = category.Id,
                        Name = category.Name,
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
