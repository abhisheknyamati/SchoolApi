using AutoMapper;
using SchoolApi.Models.DTOs;

namespace SchoolApi.Models.AutomapperHelper
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, AddStudentDto>().ReverseMap();
            //CreateMap<AddStudentDto, Student>();
        }
    }
}
