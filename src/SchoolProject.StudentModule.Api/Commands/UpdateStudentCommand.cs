using MediatR;
using SchoolProject.StudentModule.Api.DTOs;
using SchoolProject.StudentModule.Business.Models;

namespace SchoolProject.StudentModule.Api.Commands
{
    public class UpdateStudentCommand : IRequest<Student>
    {
        public int Id { get; set; }
        public UpdateStudentDto StudentDto { get; set; }

        public UpdateStudentCommand(int id, UpdateStudentDto studentDto)
        {
            Id = id;
            StudentDto = studentDto;
        }
    }
}