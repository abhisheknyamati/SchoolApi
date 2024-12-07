using MediatR;
using SchoolProject.StudentModule.Business.Models;

namespace SchoolProject.StudentModule.Api.Commands
{
    public class DeleteStudentCommand(Student student) : IRequest<Student>
    {
        public Student Student { get; set; } = student;      
    }
}