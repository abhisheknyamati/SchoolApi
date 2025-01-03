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
        private readonly IGenericReadRepository<Student> _genericReadRepo;
        public GetStudentByIdHandler(IStudentRepo _repo, IGenericRepository<Student> _genericRepo, IGenericReadRepository<Student> _genericReadRepo)
        {
            this._repo = _repo;
            this._genericRepo = _genericRepo;
            this._genericReadRepo = _genericReadRepo;
        }
        public async Task<Student?> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            // return await _repo.GetStudentById(request.Id);
            // return await _genericRepo.GetByIdAsync(request.Id);
            return await _genericReadRepo.GetByIdAsync(request.Id);
        }
    }
}