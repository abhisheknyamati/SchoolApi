using MediatR;
using SchoolProject.StudentModule.Business.Commands;
using SchoolProject.Core.Business.Repositories.Interfaces;
using SchoolProject.Core.Business.Commands;

namespace SchoolProject.StudentModule.Business.Handlers
{
    public class DeleteStudentHandler: IRequestHandler<DeleteStudentCommand, bool>
    {
        // private readonly IStudentRepo _repo;
        // public DeleteStudentHandler(IStudentRepo repo)
        // {
        //     _repo = repo;
        // }

        public async Task<bool> Handle(DeleteStudentCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();    
            // var student = await _repo.GetStudentById(command.Id);
            // if(student == null)
            // {
            //     return default;
            // }
            // return await _repo.DeleteStudent(student);  // why not generic - issue implementing soft delete
        }
    }
}