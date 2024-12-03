using MediatR;
using SchoolProject.Core.Business.Repositories.Interfaces;
using SchoolProject.StudentModule.Business.Queries;
using SchoolProject.Core.Business.Models;

namespace SchoolProject.StudentModule.Business.Handlers
{
    public class GetStudentListHandler : IRequestHandler<GetStudentListQuery, IEnumerable<Student>>
    {
        private readonly IGenericRepository<Student> _repo;
        public GetStudentListHandler(IGenericRepository<Student> _repo)
        {
            this._repo = _repo;
        }

        public async Task<IEnumerable<Student>> Handle(GetStudentListQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetAllAsync();
        }
    }
}