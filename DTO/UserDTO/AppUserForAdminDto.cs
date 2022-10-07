using PayPal.Api;
using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.RoleDTO;
using PetStoreApi.DTO.UserInfoDTO;

namespace PetStoreApi.DTO.UserDTO
{
    public class AppUserForAdminDto
    {
        public Guid Id { get ; set; }
        public string Username { get; set; }
        public string Email { get ; set; }
        public bool Enabled { get; set; }
        public ICollection<AppRoleDto> AppRoles { get; set; } = new HashSet<AppRoleDto>();
        public UserInfoResponseDto UserInfo { get; set; }
        public bool AccountNonLocked { get; set; }
        public string FullName { get; set; }

        public static AppUserForAdminDto CreateFromEntity(AppUser src)
        {
            AppUserForAdminDto dest = new AppUserForAdminDto();

            dest.Id = src.Id;
            dest.Username = src.Username;
            dest.Email = src.Email;
            dest.Enabled = src.Enabled;
            dest.AccountNonLocked = src.AccountNonLocked;

            if (src.UserInfo != null)
            {
                dest.UserInfo = UserInfoResponseDto.CreateFromEntity(src.UserInfo);
                dest.FullName = src.UserInfo.LastName + " " + src.UserInfo.FirstName;
            }
            if (src.AppUserRoles.Count > 0)
                foreach (var role in src.AppUserRoles)
                {
                    if (role != null)
                    {
                        dest.AppRoles.Add(AppRoleDto.CreateFromEntity(role.AppRole));
                    } 
                }

            return dest;
        }
    }
}
