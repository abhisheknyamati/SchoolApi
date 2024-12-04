using MediatR;
using SchoolProject.StudentModule.Business.Models;

namespace SchoolProject.StudentModule.Api.Commands
{
    public class UpdateStudentCommand : IRequest<Student>
    {
        public int Id { get; set; }
        public Student Student { get; set; }

        public UpdateStudentCommand(int id, Student student)
        {
            Id = id;
            Student = student;
        }
    }
}