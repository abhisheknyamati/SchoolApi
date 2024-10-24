
using Microsoft.AspNetCore.Mvc;
using Moq;
using SchoolApi.Models.ENUM;
using SchoolApi.Models;
using SchoolApi.Services;
using SchoolApi.Repositories;

namespace SchoolApiTests
{
    [TestClass]
    public class StudentServiceTests
    {
        private Mock<IStudentRepo> _mockRepo;
        private StudentService _studentService;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IStudentRepo>();
            _studentService = new StudentService(_mockRepo.Object);
        }

        [TestMethod]
        public async Task AddStudent_WhenCalled_ReturnCorrectStudent()
        {
            // Arrange
            var student = new Student
            {
                Id = 1,
                FirstName = "Maxx",
                LastName = "Power",
                Email = "max.power@example.com",
                Phone = "1234567890",
                Address = "vashi",
                Gender = Gender.MALE,
                BirthDate = new DateTime(2001, 1, 1),
                Age = 23,
                IsActive = true
            };

            _mockRepo.Setup(repo => repo.AddStudent(It.IsAny<Student>()))
                     .ReturnsAsync(new OkObjectResult(student));

            // Act
            var result = await _studentService.AddStudent(student);

            // Assert
            _mockRepo.Verify(repo => repo.AddStudent(It.Is<Student>(s => s == student)), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetAllStudents_WhenCalled_ReturnListOfStudnets()
        {
            //arrange
            var students = new List<Student>()
            {
                new Student {
                    Id = 1,
                    FirstName = "Maxx",
                    LastName = "Power",
                    Email = "max.power@example.com",
                    Phone = "1234567890",
                    Address = "vashi",
                    Gender = Gender.MALE,
                    BirthDate = new DateTime(2001, 1, 1),
                    Age = 23,
                    IsActive = true
                },
                new Student {
                    Id = 2,
                    FirstName = "Maxx2",
                    LastName = "Power2",
                    Email = "max.power@example.com",
                    Phone = "1234567890",
                    Address = "vashi",
                    Gender = Gender.MALE,
                    BirthDate = new DateTime(2000, 1, 1),
                    Age = 24,
                    IsActive = true
                },
            };

            _mockRepo.Setup(repo => repo.GetAllStudents()).ReturnsAsync(students);

            //act
            var result = await _studentService.GetAllStudents();

            //assert
            Assert.AreEqual(2, result.Count());
            var firstStudent = result.First();
            Assert.AreEqual("Maxx", firstStudent.FirstName);
            var lastStudent = result.Last();
            Assert.AreEqual("1234567890", lastStudent.Phone);
            Assert.AreEqual(Gender.MALE, lastStudent.Gender);
        }

        [TestMethod]
        public async Task GetAllStudents_WhenError_ReturnEmptyList()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllStudents()).ReturnsAsync((IEnumerable<Student>)null);

            // Act
            var result = await _studentService.GetAllStudents();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }


        [TestMethod]
        public async Task CalculateAge_InvalidBirthDate_ReturnInCorrectAge()
        {
            // Arrange
            //var birthDate = new DateTime(2000, 1, 1);
            var birthDate = new DateTime(2000, 10, 23);
            var expectedAge = 27;

            // Act
            var result = await _studentService.CalculateAge(birthDate);

            // Assert
            Assert.AreNotEqual(expectedAge, result);
        }

        [TestMethod]
        public async Task CalculateAge_ValidBirthDate_ReturnCorrectAge()
        {
            // Arrange
            var birthDate = new DateTime(2000, 1, 1);
            var expectedAge = 24;

            // Act
            var result = await _studentService.CalculateAge(birthDate);

            // Assert
            Assert.AreEqual(expectedAge, result);
        }

        [TestMethod]
        public async Task DeleteStudent_WithValidStudentId_ShouldCallRepoAndReturnOkResult()
        {
            // Arrange
            int studentId = 100;

            _mockRepo.Setup(repo => repo.DeleteStudent(studentId)).ReturnsAsync(new OkResult());

            // Act
            var result = await _studentService.DeleteStudent(studentId);

            // Assert
            _mockRepo.Verify(repo => repo.DeleteStudent(studentId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task DeleteStudent_WithInvalidStudentId_ShouldReturnBadRequest()
        {
            // Arrange
            int studentId = 100;

            _mockRepo.Setup(repo => repo.DeleteStudent(studentId)).ReturnsAsync(new BadRequestObjectResult(new { message = "invalid studnet id" }));

            // Act
            var result = await _studentService.DeleteStudent(studentId);

            // Assert
            _mockRepo.Verify(repo => repo.DeleteStudent(studentId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task UpdateDetails_WithValidData_ShouldReturnUpdatedStudent()
        {
            // Arrange
            var studentId = 1;
            var studentToUpdate = new Student
            {
                Id = studentId,
                FirstName = "Maxx",
                LastName = "Power",
                Email = "maxx.power@example.com",
                Phone = "0987654321",
                Address = "Updated Address",
                Gender = Gender.MALE,
                BirthDate = new DateTime(2001, 1, 1),
                Age = 23,
                IsActive = true
            };

            _mockRepo.Setup(repo => repo.UpdateDetails(studentId, studentToUpdate))
                     .ReturnsAsync(new OkObjectResult(studentToUpdate));

            // Act
            var result = await _studentService.UpdateDetails(studentId, studentToUpdate);

            // Assert
            _mockRepo.Verify(repo => repo.UpdateDetails(studentId, It.Is<Student>(s => s == studentToUpdate)), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task UpdateDetails_WithInvalidStudentId_ShouldReturnNotFound()
        {
            // Arrange
            var studentId = 99;
            var studentToUpdate = new Student
            {
                FirstName = "Maxx",
                LastName = "Power",
                Email = "maxx.power@example.com",
                Phone = "0987654321",
                Address = "Updated Address",
                Gender = Gender.MALE,
                BirthDate = new DateTime(2001, 1, 1),
                Age = 23,
                IsActive = true
            };

            _mockRepo.Setup(repo => repo.UpdateDetails(studentId, studentToUpdate)).ReturnsAsync(new NotFoundResult());

            // Act
            var result = await _studentService.UpdateDetails(studentId, studentToUpdate);

            // Assert
            _mockRepo.Verify(repo => repo.UpdateDetails(studentId, It.IsAny<Student>()), Times.Once);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetStudentById_WithValidId_ShouldReturnStudent()
        {
            // Arrange
            var studentId = 1;
            var student = new Student
            {
                Id = studentId,
                FirstName = "Maxx",
                LastName = "Power",
                Email = "max.power@example.com",
                Phone = "1234567890",
                Address = "vashi",
                Gender = Gender.MALE,
                BirthDate = new DateTime(2001, 1, 1),
                Age = 23,
                IsActive = true
            };

            _mockRepo.Setup(repo => repo.GetStudentById(studentId))
                     .ReturnsAsync(new OkObjectResult(student));

            // Act
            var result = await _studentService.GetStudentById(studentId) as ActionResult<Student>;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Student>));
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(student, okResult.Value);
        }

        [TestMethod]
        public async Task GetStudentById_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var studentId = 1;

            _mockRepo.Setup(repo => repo.GetStudentById(studentId)).ReturnsAsync(new NotFoundResult());

            // Act
            var result = await _studentService.GetStudentById(studentId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }


    }


}
