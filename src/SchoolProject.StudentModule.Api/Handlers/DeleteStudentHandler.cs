using MediatR;
using SchoolProject.StudentModule.Api.Commands;
using SchoolProject.StudentModule.Business.Repositories.Interfaces;

namespace SchoolProject.StudentModule.Api.Handlers
{
    public class DeleteStudentHandler: IRequestHandler<DeleteStudentCommand, bool>
    {
        private readonly IStudentRepo _repo;
        public DeleteStudentHandler(IStudentRepo repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteStudentCommand command, CancellationToken cancellationToken)
        {
            var student = await _repo.GetStudentById(command.Id);
            if(student == null)
            {
                return default;
            }
            return await _repo.DeleteStudent(student);
        }
    }
}