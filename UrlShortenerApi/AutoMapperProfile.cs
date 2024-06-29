using AutoMapper;
using UrlShortenerApi.Dto;
using UrlShortenerApi.Entity.DBEntities;

namespace UrlShortenerApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() { 
            CreateMap<UserEntity, UserViewDto>().ReverseMap();
            CreateMap<UserEntity, UserLoginDto>().ReverseMap();
            CreateMap<UserEntity, UserRegisterDto>().ReverseMap();

            CreateMap<LinkEntity, LinkViewDto>().ReverseMap();
            CreateMap<LinkEntity, LinkCreateDto>().ReverseMap();

        }
    }
}
