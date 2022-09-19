using PetStoreApi.Data.Entity;

namespace PetStoreApi.DTO.OriginDTO
{
    public class OriginDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public OriginDto()
        {
        }
        public static OriginDto CreateFromEntity(Origin src)
        {
            OriginDto dto = new OriginDto();

            dto.Id = src.Id;
            dto.Name = src.Name;

            return dto;
        }
    }
}
