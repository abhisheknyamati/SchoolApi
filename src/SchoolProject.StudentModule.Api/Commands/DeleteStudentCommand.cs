using MediatR;

namespace SchoolProject.StudentModule.Api.Commands
{
    public class DeleteStudentCommand : IRequest<bool>
    {
        public int Id {get; set;}
      
    }
}