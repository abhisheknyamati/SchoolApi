using MediatR;
using SchoolProject.Core.Business.Repositories.Interfaces;
using SchoolProject.Core.Business.Models;
using SchoolProject.StudentModule.Business.Queries;

namespace SchoolProject.StudentModule.Business.Handlers
{
    public class GetStudentByIdHandler : IRequestHandler<GetStudentByIdQuery, Student?>
    {
        private readonly IGenericRepository<Student> _repo;
        public GetStudentByIdHandler(IGenericRepository<Student> _repo)
        {
            this._repo = _repo;
        }
        public async Task<Student?> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetByIdAsync(request.Id);
        }
    }
}