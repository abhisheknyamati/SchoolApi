using MediatR;
using SchoolProject.StudentModule.Api.Queries;
using SchoolProject.StudentModule.Business.Models;
using SchoolProject.StudentModule.Business.Repositories.Interfaces;

namespace SchoolProject.StudentModule.Api.Handlers
{
    public class GetStudentListHandler : IRequestHandler<GetStudentListQuery, IEnumerable<Student>>
    {
        private readonly IStudentRepo _repo;
        public GetStudentListHandler(IStudentRepo _repo)
        {
            this._repo = _repo;
        }

        public async Task<IEnumerable<Student>> Handle(GetStudentListQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetAllStudents();
        }
    }
}