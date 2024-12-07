using MediatR;
using SchoolProject.Core.Business.Repositories.Interface;
using SchoolProject.StudentModule.Api.Commands;
using SchoolProject.StudentModule.Business.Models;
using SchoolProject.StudentModule.Business.Repositories.Interfaces;

namespace SchoolProject.StudentModule.Api.Handlers
{
    public class DeleteStudentHandler: IRequestHandler<DeleteStudentCommand, Student?>
    {
        private readonly IStudentRepo _repo;
        private readonly IGenericRepository<Student> _genericRepo;
        public DeleteStudentHandler(IStudentRepo repo, IGenericRepository<Student> genericRepo)
        {
            _repo = repo;
            _genericRepo = genericRepo;
        }

        public async Task<Student?> Handle(DeleteStudentCommand command, CancellationToken cancellationToken)
        {
            // await _repo.DeleteStudent(command.Student);
            var deletedStudent = await _genericRepo.SoftDeleteAsync(command.Student);
            return deletedStudent;
        }
    }
}