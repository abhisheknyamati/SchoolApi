using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolApi.API.Controllers;
using SchoolApi.Business.Models;
using SchoolApi.Business.Services;
using SchoolApi.API.DTOs;
using SchoolApi.Business.Pagination;
using SchoolApi.Business.Repositories;
using Bogus;
using SchoolApi.Business.Models.ENUM;
using SchoolApi.API.ExceptionHandler;
using Microsoft.AspNetCore.Http;

namespace SchoolApi.Tests
{
    public class StudentControllerTests
    {
        private readonly Mock<IStudentRepo> _mockRepo;
        private readonly Mock<IStudentService> _mockService;
        private readonly IMapper _mapper;
        private readonly StudentController _controller;
        private readonly Faker<Student> _studentFaker;
        private readonly Faker<AddStudentDto> _addStudentDtoFaker;

        public StudentControllerTests()
        {
            _mockRepo = new Mock<IStudentRepo>();
            _mockService = new Mock<IStudentService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddStudentDto, Student>();
                cfg.CreateMap<UpdateStudentDto, Student>();
            });
            _mapper = config.CreateMapper();

            _controller = new StudentController(_mockRepo.Object, _mapper, _mockService.Object);

            _studentFaker = new Faker<Student>()
                .RuleFor(s => s.Id, f => f.IndexFaker + 1)
                .RuleFor(s => s.FirstName, f => f.Name.FirstName())
                .RuleFor(s => s.LastName, f => f.Name.LastName())
                .RuleFor(s => s.Email, f => f.Internet.Email())
                .RuleFor(s => s.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(s => s.Address, f => f.Address.FullAddress())
                .RuleFor(s => s.BirthDate, f => f.Date.Past(20))
                .RuleFor(s => s.Gender, f => f.PickRandom<Gender>())
                .RuleFor(s => s.Age, f => f.Random.Int(18, 25));

            _addStudentDtoFaker = new Faker<AddStudentDto>()
                .RuleFor(s => s.FirstName, f => f.Name.FirstName())
                .RuleFor(s => s.LastName, f => f.Name.LastName())
                .RuleFor(s => s.Email, f => f.Internet.Email())
                .RuleFor(s => s.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(s => s.Address, f => f.Address.FullAddress())
                .RuleFor(s => s.BirthDate, f => f.Date.Past(20));
        }

        [Fact]
        public async Task GetAllStudents_ReturnsOk_WhenStudentsExist()
        {
            // Arrange
            var students = _studentFaker.Generate(5);
            _mockRepo.Setup(repo => repo.GetAllStudents()).ReturnsAsync(students);

            // Act
            var result = await _controller.GetAllStudents();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedStudents = Assert.IsAssignableFrom<IEnumerable<Student>>(okResult.Value);
            Assert.Equal(5, returnedStudents.Count());
        }

        [Fact]
        public async Task AddStudent_ReturnsOk_WhenStudentIsAddedSuccessfully()
        {
            // Arrange
            var studentDto = new AddStudentDto
            {
                FirstName = "Abhishek",
                LastName = "Nyamati",
                Email = "abhi@example.com",
                BirthDate = new DateTime(2001, 1, 1),
                Address = "Somewhere",
                Phone = "1234567890",
                Gender = Gender.MALE,
            };
            var student = _mapper.Map<Student>(studentDto);

            _mockService.Setup(service => service.CalculateAge(studentDto.BirthDate.Value)).Returns(23);
            _mockRepo.Setup(repo => repo.AddStudent(It.IsAny<Student>())).ReturnsAsync(student);

            // Act
            var result = await _controller.AddStudent(studentDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var addedStudent = Assert.IsType<Student>(okResult.Value);
            Assert.Equal("Abhishek", addedStudent.FirstName);
            Assert.Equal(23, addedStudent.Age);
        }

        [Fact]
        public async Task DeleteStudent_ReturnsOk_WhenStudentIsDeletedSuccessfully()
        {
            // Arrange
            var student = _studentFaker.Generate();
            _mockRepo.Setup(repo => repo.GetStudentById(student.Id)).ReturnsAsync(student);
            _mockRepo.Setup(repo => repo.DeleteStudent(student)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteStudent(student.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var deletedStudent = Assert.IsType<Student>(okResult.Value);
            Assert.Equal(student.Id, deletedStudent.Id);
        }

        [Fact]
        public async Task UpdateDetails_ReturnsOk_WhenStudentIsUpdatedSuccessfully()
        {
            // Arrange
            var student = _studentFaker.Generate();
            var studentDto = new UpdateStudentDto
            {
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName"
            };

            _mockRepo.Setup(repo => repo.GetStudentById(student.Id)).ReturnsAsync(student);
            _mockRepo.Setup(repo => repo.UpdateDetails(student)).ReturnsAsync(student);

            // Act
            var result = await _controller.UpdateDetails(student.Id, studentDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedStudent = Assert.IsType<Student>(okResult.Value);
            Assert.Equal("UpdatedFirstName", updatedStudent.FirstName);
            Assert.Equal("UpdatedLastName", updatedStudent.LastName);
        }

        [Fact]
        public async Task GetStudentById_ReturnsOk_WhenStudentExists()
        {
            // Arrange
            var student = _studentFaker.Generate();
            _mockRepo.Setup(repo => repo.GetStudentById(student.Id)).ReturnsAsync(student);

            // Act
            var result = await _controller.GetStudentById(student.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var foundStudent = Assert.IsType<Student>(okResult.Value);
            Assert.Equal(student.Id, foundStudent.Id);
        }

        [Fact]
        public async Task GetPagedStudents_ReturnsOk_WithValidPagination()
        {
            // Arrange
            var students = _studentFaker.Generate(5);
            var pagedResponse = new PagedResponse<Student>(students, 1, 5, 10);

            _mockRepo.Setup(repo => repo.GetStudents(1, 5, "")).ReturnsAsync(pagedResponse);

            // Act
            var result = await _controller.GetPagedStudents(pageNumber: 1, pageSize: 5);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPagedResponse = Assert.IsType<PagedResponse<Student>>(okResult.Value);
            Assert.Equal(5, returnedPagedResponse.Data.Count);
            Assert.Equal(10, returnedPagedResponse.TotalRecords);
        }

        // -ve test cases

        [Fact]
        public async Task GetAllStudents_ReturnsNotFound_WhenNoStudentsExist()
        {
            // Arrange
            var students = new List<Student>();
            _mockRepo.Setup(repo => repo.GetAllStudents()).ReturnsAsync(students);

            // Act
            var result = await _controller.GetAllStudents();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(ErrorMsgConstant.StudentListEmpty, notFoundResult.Value);
        }

        [Fact]
        public async Task AddStudent_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var studentDto = _addStudentDtoFaker.Generate();
            studentDto.Email = null;
            
            // Act
            var result = await _controller.AddStudent(studentDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsAssignableFrom<IEnumerable<string>>(badRequestResult.Value);
            Assert.NotEmpty(errors);
        }

        
    }
}
