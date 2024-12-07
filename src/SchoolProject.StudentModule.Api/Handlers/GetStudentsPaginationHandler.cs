
using MediatR;
using SchoolProject.StudentModule.Api.Queries;
using SchoolProject.StudentModule.Business.Models;
using SchoolProject.StudentModule.Business.Pagination;
using SchoolProject.StudentModule.Business.Repositories.Interfaces;

namespace SchoolProject.StudentModule.Api.Handlers
{
    public class GetStudentsPaginationHandler(IStudentRepo repo) : IRequestHandler<GetStudentsPaginationQuery, PagedResponse<Student>>
    {
        private readonly IStudentRepo _repo = repo;

        public Task<PagedResponse<Student>> Handle(GetStudentsPaginationQuery request, CancellationToken cancellationToken)
        {
            return _repo.GetStudents(request.PageNumber, request.PageSize, request.SearchTerm ?? string.Empty);
        }
    }
}