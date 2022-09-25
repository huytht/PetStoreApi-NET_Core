using PetStoreApi.Data.Entity;

namespace PetStoreApi.DTO.BreedDTO
{
    public class BreedDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public BreedDto()
        {
        }
        public static BreedDto CreateFromEntity(Breed src)
        {
            BreedDto dto = new BreedDto();

            dto.Id = src.Id;
            dto.Name = src.Name;

            return dto;
        }
    }
}
