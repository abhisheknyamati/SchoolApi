using MediatR;
using SchoolProject.StudentModule.Business.Models;

namespace SchoolProject.StudentModule.Api.Queries
{
    public class GetStudentByIdQuery : IRequest<Student>
    {
        public int Id {get; set;}
    }
}