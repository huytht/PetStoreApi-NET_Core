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
    }
}
