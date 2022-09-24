using AutoMapper;
using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.UserDTO;

namespace PetStoreApi.Domain
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, UserRegisterDto>().ReverseMap();
            CreateMap<AppUser, UserLoginResponseDto>().ReverseMap();
        }
    }
}
