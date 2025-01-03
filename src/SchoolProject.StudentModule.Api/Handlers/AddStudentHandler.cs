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
        private readonly IGenericWriteRepository<Student> _genericWriteRepo;
        public AddStudentHandler(IStudentRepo repo, IGenericRepository<Student> genericRepo, IGenericWriteRepository<Student> genericWriteRepo)
        {
            _repo = repo;
            _genericRepo = genericRepo;
            _genericWriteRepo = genericWriteRepo;
        }
        public Task<Student> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            // return _repo.AddStudent(request.Student);
            // return _genericRepo.AddAsync(request.Student);
            return _genericWriteRepo.AddAsync(request.Student);
        }
    }
}
