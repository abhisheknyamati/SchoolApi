using AutoMapper;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SchoolApi.API.Controllers;
using SchoolApi.API.DTOs;
using SchoolApi.Business.Models;
using SchoolApi.Business.Services;
using SchoolApi.Business.Repositories;
using System.Net;
using SchoolApi.API.ExceptionHandler;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace SchoolApi.Test
{
    public class StudentControllerTest
    {
        private readonly Mock<IStudentService> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IStudentRepo> _mockRepo;
        private readonly StudentController _controller;
        private readonly Faker<Student> _studentFaker;
        private readonly Faker<AddStudentDto> _addStudentDtoFaker;

        public StudentControllerTest()
        {
            _mockService = new Mock<IStudentService>();
            _mockMapper = new Mock<IMapper>();
            _mockRepo = new Mock<IStudentRepo>();
            _controller = new StudentController(_mockRepo.Object, _mockMapper.Object, _mockService.Object);

            _studentFaker = new Faker<Student>()
                .RuleFor(s => s.Id, f => f.IndexFaker + 1)
                .RuleFor(s => s.FirstName, f => f.Name.FirstName())
                .RuleFor(s => s.LastName, f => f.Name.LastName())
                .RuleFor(s => s.Email, f => f.Internet.Email())
                .RuleFor(s => s.Address, f => f.Address.FullAddress())
                .RuleFor(s => s.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(s => s.Gender, f => f.PickRandom<Business.Models.ENUM.Gender>())
                .RuleFor(s => s.BirthDate, f => f.Date.Past(20, DateTime.Now.AddYears(-10)))
                .RuleFor(s => s.Age, (f, s) => DateTime.Now.Year - s.BirthDate.Year)
                .RuleFor(s => s.IsActive, true);

            _addStudentDtoFaker = new Faker<AddStudentDto>()
                .RuleFor(d => d.FirstName, f => f.Name.FirstName())
                .RuleFor(d => d.LastName, f => f.Name.LastName())
                .RuleFor(d => d.Email, f => f.Internet.Email())
                .RuleFor(d => d.Address, f => f.Address.FullAddress())
                .RuleFor(d => d.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(d => d.Gender, f => f.PickRandom<Business.Models.ENUM.Gender>())
                .RuleFor(d => d.BirthDate, f => f.Date.Past(20, DateTime.Now.AddYears(-10)));
        }

        [Fact]
        public async Task GetAllStudents_ReturnsOkResult_WithListOfStudents()
        {
            // Arrange
            var students = _studentFaker.Generate(5);
            _mockRepo.Setup(s => s.GetAllStudents()).ReturnsAsync(students);

            // Act
            var result = await _controller.GetAllStudents();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnStudents = Assert.IsType<List<Student>>(okResult.Value);
            Assert.Equal(5, returnStudents.Count);
        }

        [Fact]
        public async Task AddStudent_ReturnsCreatedResult_WithStudent()
        {
            // Arrange
            var studentDto = _addStudentDtoFaker.Generate();
            studentDto.Email = "validemail@example.com"; 

            var student = _studentFaker.Generate();

            _mockMapper.Setup(m => m.Map<Student>(studentDto)).Returns(student);
            _mockRepo.Setup(s => s.AddStudent(It.IsAny<Student>())).ReturnsAsync(student);

            // Act
            var result = await _controller.AddStudent(studentDto);

            // Assert
            var createdResult = Assert.IsType<OkObjectResult>(result); 
            var returnStudent = Assert.IsType<Student>(createdResult.Value);
            Assert.Equal(student.FirstName, returnStudent.FirstName);
            Assert.Equal(student.LastName, returnStudent.LastName);
            Assert.Equal(student.Id, returnStudent.Id); 
        }


        [Fact]
        public async Task GetStudentById_ReturnsOkResult_WithStudent()
        {
            // Arrange
            var student = _studentFaker.Generate();
            _mockRepo.Setup(s => s.GetStudentById(student.Id)).ReturnsAsync(student);

            // Act
            var result = await _controller.GetStudentById(student.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnStudent = Assert.IsType<Student>(okResult.Value);
            Assert.Equal(student.Id, returnStudent.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFoundResult_WhenStudentDoesNotExist()
        {
            // Arrange
            var nonExistentStudentId = 999;

            // Setup the repository to throw a KeyNotFoundException
            _mockRepo
                .Setup(repo => repo.GetStudentById(nonExistentStudentId))
                .ThrowsAsync(new KeyNotFoundException(ErrorMsgConstant.StudentNotFound));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _controller.GetStudentById(nonExistentStudentId));

            // Assert the exception message
            Assert.Equal(ErrorMsgConstant.StudentNotFound, exception.Message);
        }

        [Fact]
        public async Task DeleteStudent_ReturnsOkResult_WhenStudentIsDeleted()
        {
            // Arrange
            var student = _studentFaker.Generate();
            _mockRepo.Setup(s => s.GetStudentById(student.Id)).ReturnsAsync(student);
            _mockRepo.Setup(s => s.DeleteStudent(student)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteStudent(student.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(student.Id, ((Student)okResult.Value).Id);
        }
    }
}
