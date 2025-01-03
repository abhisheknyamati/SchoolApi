using MediatR;
using SchoolProject.Core.Business.Repositories.Interface;
using SchoolProject.StudentModule.Api.Queries;
using SchoolProject.StudentModule.Business.Models;
using SchoolProject.StudentModule.Business.Repositories.Interfaces;

namespace SchoolProject.StudentModule.Api.Handlers
{
    public class GetStudentListHandler : IRequestHandler<GetStudentListQuery, IEnumerable<Student>>
    {
        private readonly IStudentRepo _repo;
        private readonly IGenericRepository<Student> _genericRepo;
        private readonly IGenericReadRepository<Student> _genericReadRepo;
        public GetStudentListHandler(IStudentRepo _repo, IGenericRepository<Student> _genericRepo, IGenericReadRepository<Student> _genericReadRepo)
        {
            this._repo = _repo;
            this._genericRepo = _genericRepo;
            this._genericReadRepo = _genericReadRepo;
        }

        public async Task<IEnumerable<Student>> Handle(GetStudentListQuery request, CancellationToken cancellationToken)
        {
            // return await _repo.GetAllStudents();
            // return await _genericRepo.GetAllAsync();
            return await _genericReadRepo.GetAllAsync();
        }
    }
}