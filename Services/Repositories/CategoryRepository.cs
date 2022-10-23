using Microsoft.EntityFrameworkCore;
using PetStoreApi.Data.Entity;
using PetStoreApi.Helpers;

namespace PetStoreApi.Services.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context) : base(context)
        {
        }

        public void CreateCategory(Category category)
        {
            Create(category);
        }

        public void DeleteCategory(Category category)
        {
            Delete(category);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await FindAll().OrderBy(b => b.Name).ToListAsync();
        }

        public void UpdateCategory(Category category)
        {
            Update(category);
        }

        public async Task<Category> GetCategory(int? id)
        {
            return await FindByCondition(b => b.Id.Equals(id)).FirstOrDefaultAsync();
        }
    }
}
