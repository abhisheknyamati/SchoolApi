using MediatR;
using SchoolProject.Core.Business.Models;

namespace SchoolProject.StudentModule.Business.Queries
{
    public class GetStudentByIdQuery : IRequest<Student?>
    {
        public int Id {get; set;}
    }
}