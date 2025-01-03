
using MediatR;
using SchoolProject.Core.Business.Repositories.Interface;
using SchoolProject.StudentModule.Api.Commands;
using SchoolProject.StudentModule.Business.Models;
using SchoolProject.StudentModule.Business.Repositories.Interfaces;

namespace SchoolProject.StudentModule.Api.Handlers
{
    public class UpdateHandler : IRequestHandler<UpdateCommand, Student>
    {
        private readonly IGenericWriteRepository<Student> _repo;
        public UpdateHandler(IGenericWriteRepository<Student> repo)
        {
            _repo = repo;
        }
        public async Task<Student> Handle(UpdateCommand command, CancellationToken cancellationToken)
        {
            var existingStudent = command.ExistingStudent;
            var studentDto = command.NewStudent;

            // if (!string.IsNullOrEmpty(studentDto.FirstName))
            //     existingStudent.FirstName = studentDto.FirstName;
            // if (!string.IsNullOrEmpty(studentDto.LastName))
            //     existingStudent.LastName = studentDto.LastName;
            // if (!string.IsNullOrEmpty(studentDto.Email))
            //     existingStudent.Email = studentDto.Email;
            // if (!string.IsNullOrEmpty(studentDto.Phone))
            //     existingStudent.Phone = studentDto.Phone;
            // if (!string.IsNullOrEmpty(studentDto.Address))
            //     existingStudent.Address = studentDto.Address;
            // if (!string.IsNullOrEmpty(studentDto.Gender.ToString()))
            //     existingStudent.Gender = studentDto.Gender;
            // if (!string.IsNullOrEmpty(studentDto.BirthDate.ToString()))
            //     existingStudent.BirthDate = studentDto.BirthDate;

            var success = await _repo.UpdateAsync(existingStudent, studentDto);
            return success;
        }
    }
}