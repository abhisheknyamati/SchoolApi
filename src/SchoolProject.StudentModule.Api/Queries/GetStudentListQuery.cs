using MediatR;
using SchoolProject.StudentModule.Business.Models;

namespace SchoolProject.StudentModule.Api.Queries
{
    public class GetStudentListQuery : IRequest<IEnumerable<Student>>
    {
    }
}