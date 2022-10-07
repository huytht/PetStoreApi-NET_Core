using PetStoreApi.Data.Entity;

namespace PetStoreApi.DTO.UserInfoDTO
{
    public class UserInfoResponseDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AvatarImg { get; set; }

        public static UserInfoResponseDto CreateFromEntity(UserInfo src)
        {
            UserInfoResponseDto dto = new UserInfoResponseDto();
            dto.AvatarImg = src.AvatarImg;
            if (src.AppUser != null)
            {
                dto.UserId = src.AppUser.Id;
                dto.Email = src.AppUser.Email;
            }
            dto.FirstName = src.FirstName;
            dto.LastName = src.LastName;
            dto.Phone = src.Phone;
            dto.AvatarImg = src.AvatarImg;

            return dto;
        }
    }
}
