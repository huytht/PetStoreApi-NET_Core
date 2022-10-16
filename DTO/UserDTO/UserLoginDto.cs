using System.ComponentModel.DataAnnotations;

namespace PetStoreApi.DTO.UserDTO
{
    public class UserLoginDto
    {
        [Required]
        [MinLength(length: 2, ErrorMessage = "username should have at least 2 characters")]
        public string Username { get; set; }
        [Required]
        [MinLength(length: 8, ErrorMessage = "password should have at least 8 characters")]
        public string Password { get; set; }
        public UserLoginDto()
        {
        }
    }
}
