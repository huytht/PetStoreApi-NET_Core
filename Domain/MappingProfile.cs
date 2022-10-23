using AutoMapper;
using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.BreedDTO;
using PetStoreApi.DTO.CategoryDTO;
using PetStoreApi.DTO.MomoDTO;
using PetStoreApi.DTO.OriginDTO;
using PetStoreApi.DTO.UserDTO;

namespace PetStoreApi.Domain
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, UserRegisterDto>().ReverseMap();
            CreateMap<AppUser, UserLoginResponseDto>().ReverseMap();
            CreateMap<Breed, BreedDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Origin, OriginDto>().ReverseMap();
            CreateMap<BreedEditDto, Breed>().ReverseMap();
            CreateMap<CategoryEditDto, Category>().ReverseMap();
            CreateMap<OriginEditDto, Origin>().ReverseMap();
        }
    }
}
