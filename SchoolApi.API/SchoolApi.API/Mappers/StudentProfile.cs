using AutoMapper;
using SchoolApi.API.DTOs;
using SchoolApi.Business.Models;

namespace SchoolApi.API.Mappers
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, AddStudentDto>().ReverseMap();
        }
    }
}
