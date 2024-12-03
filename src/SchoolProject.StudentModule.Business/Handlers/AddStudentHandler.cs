using MediatR;
using SchoolProject.Core.Business.Repositories.Interfaces;
using SchoolProject.StudentModule.Business.Commands;
using SchoolProject.Core.Business.Models;

namespace SchoolProject.StudentModule.Business.Handlers
{
    public class AddStudentHandler : IRequestHandler<AddStudentCommand, Student>
    {
        private readonly IGenericRepository<Student> _repo;
        public AddStudentHandler(IGenericRepository<Student> repo)
        {
            _repo = repo;
        }
        public Task<Student> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            return _repo.AddAsync(request.student);
        }
    }
}
