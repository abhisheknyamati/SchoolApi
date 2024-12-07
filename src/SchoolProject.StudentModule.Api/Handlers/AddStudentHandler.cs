using MediatR;
using SchoolProject.Core.Business.Repositories.Interface;
using SchoolProject.StudentModule.Api.Commands;
using SchoolProject.StudentModule.Business.Models;
using SchoolProject.StudentModule.Business.Repositories.Interfaces;

namespace SchoolProject.StudentModule.Api.Handlers
{
    public class AddStudentHandler : IRequestHandler<AddStudentCommand, Student>
    {
        private readonly IStudentRepo _repo;
        private readonly IGenericRepository<Student> _genericRepo;
        public AddStudentHandler(IStudentRepo repo, IGenericRepository<Student> genericRepo)
        {
            _repo = repo;
            _genericRepo = genericRepo;
        }
        public Task<Student> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            // return _repo.AddStudent(request.Student);
            return _genericRepo.AddAsync(request.Student);
        }
    }
}
