using AutoMapper;
using SchoolProject.UserModule.Api.DTOs;
using SchoolProject.UserModule.Business.Models;

namespace SchoolProject.StudentModule.Api.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, GetUserDto>().ReverseMap();
            CreateMap<User, PostUserDto>().ReverseMap();
        }
    }
}
