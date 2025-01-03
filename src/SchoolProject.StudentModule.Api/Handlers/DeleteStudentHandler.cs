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
        private readonly IGenericWriteRepository<Student> _genericWriteRepo;
        public DeleteStudentHandler(IStudentRepo repo, IGenericRepository<Student> genericRepo, IGenericWriteRepository<Student> genericWriteRepo)
        {
            _repo = repo;
            _genericRepo = genericRepo;
            _genericWriteRepo = genericWriteRepo;
        }

        public async Task<Student?> Handle(DeleteStudentCommand command, CancellationToken cancellationToken)
        {
            // await _repo.DeleteStudent(command.Student);
            var deletedStudent = await _genericWriteRepo.SoftDeleteAsync(command.Student);
            return deletedStudent;
        }
    }
}