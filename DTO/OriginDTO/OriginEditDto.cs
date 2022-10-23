using PetStoreApi.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace PetStoreApi.DTO.OriginDTO
{
    public class OriginEditDto
    {
        [Required]
        public string Name { get; set; }

        public OriginEditDto()
        {
        }
    }
}
