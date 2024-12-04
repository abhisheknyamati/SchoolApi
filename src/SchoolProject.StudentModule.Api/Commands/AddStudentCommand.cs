using MediatR;
using SchoolProject.StudentModule.Business.Models;

namespace SchoolProject.StudentModule.Api.Commands
{
    public class AddStudentCommand : IRequest<Student>
    {
        public Student Student { get; set; }

        public AddStudentCommand(Student student)
        {
            Student = student;
        }
    }
}