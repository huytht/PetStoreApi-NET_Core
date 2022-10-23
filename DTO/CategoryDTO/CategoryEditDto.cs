using PetStoreApi.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace PetStoreApi.DTO.CategoryDTO
{
    public class CategoryEditDto
    {
        [Required]
        public string Name { get; set; }

        public CategoryEditDto()
        {
        }
    }
}
