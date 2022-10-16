using System.ComponentModel.DataAnnotations;

namespace PetStoreApi.DTO.UserDTO
{
    public class ChangePassword
    {
        [Required]
        [MinLength(length: 2, ErrorMessage = "username should have at least 2 characters")]
        public string Username { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        [MinLength(length: 8, ErrorMessage = "password should have at least 8 characters")]
        public string NewPassword { get; set; }
        public ChangePassword()
        {
        }
        public ChangePassword(string Username, string OldPassword, string NewPassword)
        {
            this.Username = Username;
            this.OldPassword = OldPassword;
            this.NewPassword = NewPassword;
        }
    }
}
