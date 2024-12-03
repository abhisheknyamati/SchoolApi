using MediatR;
using SchoolProject.Core.Business.Models;

namespace SchoolProject.StudentModule.Business.Commands
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