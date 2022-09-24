using AutoMapper;
using PetStoreApi.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PetStoreApi.DTO.UserDTO
{
    public class UserRegisterDto
    {
        [Required]
        [MinLength(length: 2, ErrorMessage = "user name should have at least 2 characters")]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(length: 8, ErrorMessage = "password should have at least 8 characters")]
        public string Password { get; set; }

       
    }
}
