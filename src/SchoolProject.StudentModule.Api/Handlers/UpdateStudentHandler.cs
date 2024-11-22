
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
    public class UpdateStudentHandler : IRequestHandler<UpdateStudentCommand, Student>
    {

        private readonly IStudentRepo _repo;
        private readonly IStudentService _service;
        private readonly IMapper _mapper;

        public UpdateStudentHandler(IStudentRepo repo, IStudentService service, IMapper mapper)
        {
            _repo = repo;
            _service = service;
            _mapper = mapper;
        }
        public async Task<Student> Handle(UpdateStudentCommand command, CancellationToken cancellationToken)
        {
            var existingStudent = await _repo.GetStudentById(command.Id);
            if (existingStudent == null)
            {
                // return NotFound(ErrorMsgConstant.StudentNotFound);   // idk why error in exception
                throw new Exception(ErrorMsgConstant.StudentNotFound);
            }

            var studentDto = command.StudentDto;

            if (!string.IsNullOrEmpty(studentDto.FirstName))
                existingStudent.FirstName = studentDto.FirstName;
            if (!string.IsNullOrEmpty(studentDto.LastName))
                existingStudent.LastName = studentDto.LastName;
            if (!string.IsNullOrEmpty(studentDto.Email))
            {
                if (_repo.IsDuplicateEmail(studentDto.Email))
                {
                    throw new EmailAlreadyRegistered(ErrorMsgConstant.EmailAlreadyExists);
                }
                existingStudent.Email = studentDto.Email;
            }
            if (!string.IsNullOrEmpty(studentDto.Phone))
                existingStudent.Phone = studentDto.Phone;
            if (!string.IsNullOrEmpty(studentDto.Address))
                existingStudent.Address = studentDto.Address;
            if (studentDto.Gender.HasValue)
                existingStudent.Gender = studentDto.Gender.Value;
            if (studentDto.BirthDate.HasValue)
            {
                existingStudent.BirthDate = studentDto.BirthDate.Value;
                existingStudent.Age = _service.CalculateAge(studentDto.BirthDate.Value);
            }

            var success = await _repo.UpdateDetails(existingStudent);
            return success;
        }
    }
}