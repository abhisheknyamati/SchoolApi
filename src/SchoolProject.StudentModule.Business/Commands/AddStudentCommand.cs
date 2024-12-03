using MediatR;
using SchoolProject.Core.Business.Models;

namespace SchoolProject.StudentModule.Business.Commands
{
    public class AddStudentCommand : IRequest<Student>
    {
        public Student student { get; set; }

        public AddStudentCommand(Student student)
        {
            this.student = student;
        }
    }
}