using MediatR;
using SchoolProject.StudentModule.Business.Models;

namespace SchoolProject.StudentModule.Api.Commands
{
    public class UpdateCommand(Student student, Student existingStudent) : IRequest<Student> // primary constructor hai ye
    {
        public Student NewStudent { get; set; } = student;
        public Student ExistingStudent { get; set; } = existingStudent;
    }
}