using AutoMapper;
using ValconLibrary.DTOs;
using ValconLibrary.Entities;

namespace ValconLibrary.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserDto, UserIdentity>().ReverseMap();
            CreateMap<AuthorDetailsDto, Author>().ReverseMap();
            CreateMap<CreateBookDto, Book>().ReverseMap();
            CreateMap<BookDto, Book>().ReverseMap();
            CreateMap<BookAuthorDto, Author>().ReverseMap();
            CreateMap<UserDetailsDto, UserIdentity>().ReverseMap();
            CreateMap<BookRentHistory, Book>().ReverseMap();
        }
    }
}
