using AutoMapper;
using PetStoreApi.Data.Entity;
using PetStoreApi.Domain;

namespace PetStoreApi.DTO.UserDTO
{
    public class UserLoginResponseDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string AvatarImg { get; set; }
        public TokenModel Token { get; set; }

        public UserLoginResponseDto()
        {
        }
    }
}
