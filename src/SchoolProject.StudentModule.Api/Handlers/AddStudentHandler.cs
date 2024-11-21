using MediatR;
using SchoolProject.StudentModule.Api.Commands;
using SchoolProject.StudentModule.Business.Models;
using SchoolProject.StudentModule.Business.Repositories.Interfaces;

namespace SchoolProject.StudentModule.Api.Handlers
{
    public class AddStudentHandler : IRequestHandler<AddStudentCommand, Student>
    {
        private readonly IStudentRepo _repo;
        public AddStudentHandler(IStudentRepo repo)
        {
            _repo = repo;
        }
        public Task<Student> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            var student = new Student
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                Address = request.Address,
                IsActive = request.IsActive,
                Age = request.Age,
                BirthDate = request.BirthDate,
                Gender = request.Gender,
            };

            return _repo.AddStudent(student);
        }
    }
}
