using System.ComponentModel.DataAnnotations;

namespace PetStoreApi.DTO.UserDTO
{
    public class UserLoginDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
        public UserLoginDto()
        {
        }
    }
}
