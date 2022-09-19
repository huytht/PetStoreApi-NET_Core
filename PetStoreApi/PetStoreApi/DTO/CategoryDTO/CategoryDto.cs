using PetStoreApi.Data.Entity;

namespace PetStoreApi.DTO.CategoryDTO
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CategoryDto()
        {
        }
        public static CategoryDto CreateFromEntity(Category src)
        {
            CategoryDto dto = new CategoryDto();

            dto.Id = src.Id;
            dto.Name = src.Name;

            return dto;
        }
    }
}
