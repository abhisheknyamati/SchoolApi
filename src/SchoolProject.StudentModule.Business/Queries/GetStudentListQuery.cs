using MediatR;
using SchoolProject.Core.Business.Models;

namespace SchoolProject.StudentModule.Business.Queries
{
    public class GetStudentListQuery : IRequest<IEnumerable<Student>>
    {
    }
}