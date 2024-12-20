
using AutoMapper;
using MediatR;
using SchoolProject.Core.Business.Constants;
using SchoolProject.Core.Business.ExceptionHandler;
using SchoolProject.StudentModule.Api.Commands;
using SchoolProject.StudentModule.Business.Models;
using SchoolProject.StudentModule.Business.Repositories.Interfaces;
using SchoolProject.StudentModule.Business.Services.Interfaces;

namespace SchoolProject.StudentModule.Api.Handlers
{
    public class UpdateStudentHandler : IRequestHandler<UpdateStudentCommand, Student?>
    {

        private readonly IStudentRepo _repo;
        private readonly IStudentService _service;

        public UpdateStudentHandler(IStudentRepo repo, IStudentService service)
        {
            _repo = repo;
            _service = service;
        }
        public async Task<Student?> Handle(UpdateStudentCommand command, CancellationToken cancellationToken)
        {
            var existingStudent = await _repo.GetStudentById(command.Id);

            var studentDto = command.Student;

            if (existingStudent != null)
            {
                if (!string.IsNullOrEmpty(studentDto.FirstName))
                    existingStudent.FirstName = studentDto.FirstName;
                if (!string.IsNullOrEmpty(studentDto.LastName))
                    existingStudent.LastName = studentDto.LastName;
                if (!string.IsNullOrEmpty(studentDto.Email))
                    existingStudent.Email = studentDto.Email;
                if (!string.IsNullOrEmpty(studentDto.Phone))
                    existingStudent.Phone = studentDto.Phone;
                if (!string.IsNullOrEmpty(studentDto.Address))
                    existingStudent.Address = studentDto.Address;
                if (!string.IsNullOrEmpty(studentDto.Gender.ToString()))
                    existingStudent.Gender = studentDto.Gender;
                if (!string.IsNullOrEmpty(studentDto.BirthDate.ToString()))
                    existingStudent.BirthDate = studentDto.BirthDate;

                var success = await _repo.UpdateDetails(existingStudent);
                return success;
            }
            return null;
        }
    }
}