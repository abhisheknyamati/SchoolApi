using MediatR;
using SchoolProject.StudentModule.Business.Models;
using SchoolProject.StudentModule.Business.Pagination;

namespace SchoolProject.StudentModule.Api.Queries
{
    public class GetStudentsPaginationQuery : IRequest<PagedResponse<Student>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SearchTerm { get; set; }
        public GetStudentsPaginationQuery(){}
    }
}