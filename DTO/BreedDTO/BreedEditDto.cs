using PetStoreApi.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace PetStoreApi.DTO.BreedDTO
{
    public class BreedEditDto
    {
        [Required]
        public string Name { get; set; }

        public BreedEditDto()
        {
        }
    }
}
