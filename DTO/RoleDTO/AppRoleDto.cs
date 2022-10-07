using PetStoreApi.Data.Entity;

namespace PetStoreApi.DTO.RoleDTO
{
    public class AppRoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AppRoleDto()
        {
        }
        public static AppRoleDto CreateFromEntity(AppRole src)
        {
            AppRoleDto dto = new AppRoleDto();

            dto.Id = src.Id;
            dto.Name = src.Name;

            return dto;
        }
    }
}
