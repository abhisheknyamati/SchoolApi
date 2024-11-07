using AutoMapper;
using SchoolProject.StudentModule.Api.DTOs;
using SchoolProject.StudentModule.Business.Models;

namespace SchoolProject.StudentModule.Api.Mappers
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, AddStudentDto>().ReverseMap();
        }
    }
}
