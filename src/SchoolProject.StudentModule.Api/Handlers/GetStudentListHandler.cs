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
        public GetStudentListHandler(IStudentRepo _repo, IGenericRepository<Student> _genericRepo)
        {
            this._repo = _repo;
            this._genericRepo = _genericRepo;
        }

        public async Task<IEnumerable<Student>> Handle(GetStudentListQuery request, CancellationToken cancellationToken)
        {
            // return await _repo.GetAllStudents();
            return await _genericRepo.GetAllAsync();
        }
    }
}