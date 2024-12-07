using MediatR;
using SchoolProject.Core.Business.Repositories.Interface;
using SchoolProject.StudentModule.Api.Queries;
using SchoolProject.StudentModule.Business.Models;
using SchoolProject.StudentModule.Business.Repositories.Interfaces;

namespace SchoolProject.StudentModule.Api.Handlers
{
    public class GetStudentByIdHandler : IRequestHandler<GetStudentByIdQuery, Student?>
    {
        private readonly IStudentRepo _repo;
        private readonly IGenericRepository<Student> _genericRepo;
        public GetStudentByIdHandler(IStudentRepo _repo, IGenericRepository<Student> _genericRepo)
        {
            this._repo = _repo;
            this._genericRepo = _genericRepo;
        }
        public async Task<Student?> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            // return await _repo.GetStudentById(request.Id);
            return await _genericRepo.GetByIdAsync(request.Id);
        }
    }
}