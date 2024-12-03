using MediatR;

namespace SchoolProject.Core.Business.Commands
{
    public class DeleteStudentCommand : IRequest<bool>
    {
        public int Id {get; set;}
      
    }
}