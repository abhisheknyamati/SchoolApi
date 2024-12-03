using AutoMapper;
using SchoolProject.StudentModule.Api.DTOs;
using SchoolProject.Core.Business.Models;

namespace SchoolProject.StudentModule.Api.Mappers
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, AddStudentDto>().ReverseMap();
            CreateMap<Student, GetStudentDto>().ReverseMap();
            CreateMap<Student, UpdateStudentDto>().ReverseMap();
        }
    }
}
