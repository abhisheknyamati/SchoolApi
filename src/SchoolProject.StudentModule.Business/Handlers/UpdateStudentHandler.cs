
// using MediatR;
// using SchoolProject.Core.Business.Constants;
// using SchoolProject.Core.Business.ExceptionHandler;
// using SchoolProject.Core.Business.Repositories.Interfaces;
// using SchoolProject.Core.Business.Services.Interfaces;
// using SchoolProject.StudentModule.Business.Commands;
// using SchoolProject.StudentModule.Business.Models;
// // using SchoolProject.StudentModule.Business.Repositories.Interfaces;
// // using SchoolProject.StudentModule.Business.Services.Interfaces;

// namespace SchoolProject.StudentModule.Business.Handlers 
// {
//     public class UpdateStudentHandler : IRequestHandler<UpdateStudentCommand, Student> // NOT IMPLEMENTED PROPERLY!! 
//     {

//         private readonly IGenericRepository<Student> _repo;
//         private readonly IStudentRepo _studentRepo;
//         private readonly IStudentService _service;
//         // private readonly IMapper _mapper;

//         public UpdateStudentHandler(IGenericRepository<Student> repo, IStudentService service, IStudentRepo studentRepo)
//         {
//             _repo = repo;
//             _service = service;
//             _studentRepo = studentRepo;
//         }
//         public async Task<Student> Handle(UpdateStudentCommand command, CancellationToken cancellationToken)
//         {

//             throw new NotImplementedException();
//             // var existingStudent = await _repo.GetByIdAsync(command.Id);
//             // if (existingStudent == null)
//             // {
//             //     // return NotFound(ErrorMsgConstant.StudentNotFound);   // idk why error in exception
//             //     throw new Exception(ErrorMsgConstant.StudentNotFound);
//             // }

//             // var studentDto = command.Student;

//             // if (!string.IsNullOrEmpty(studentDto.FirstName))
//             //     existingStudent.FirstName = studentDto.FirstName;
//             // if (!string.IsNullOrEmpty(studentDto.LastName))
//             //     existingStudent.LastName = studentDto.LastName;
//             // if (!string.IsNullOrEmpty(studentDto.Email))
//             // {
//             //     if (_studentRepo.IsDuplicateEmail(studentDto.Email))
//             //     {
//             //         throw new EmailAlreadyRegistered(ErrorMsgConstant.EmailAlreadyExists);
//             //     }
//             //     existingStudent.Email = studentDto.Email;
//             // }
//             // if (!string.IsNullOrEmpty(studentDto.Phone))
//             //     existingStudent.Phone = studentDto.Phone;
//             // if (!string.IsNullOrEmpty(studentDto.Address))
//             //     existingStudent.Address = studentDto.Address;
//             // // if (studentDto.Gender.HasValue)
//             // //     existingStudent.Gender = studentDto.Gender.Value;
//             // // if (studentDto.BirthDate.HasValue)
//             // // {
//             // //     existingStudent.BirthDate = studentDto.BirthDate.Value;
//             // //     existingStudent.Age = _service.CalculateAge(studentDto.BirthDate.Value);
//             // // }

//             // var success = await _repo.UpdateAsync(existingStudent);
//             // return success;
//         }
//     }
// }