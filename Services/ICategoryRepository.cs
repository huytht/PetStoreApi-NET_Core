using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.CategoryDTO;

namespace PetStoreApi.Services
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategory(int? id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        void CreateCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
    }
}
