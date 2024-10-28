using Moq;
using SchoolApi.Business.Models.ENUM;
using SchoolApi.Business.Models;
using SchoolApi.Business.Repositories;
using SchoolApi.Business.Services;

namespace SchoolApi.Test
{
    public class StudentServiceTest
    {
        private readonly Mock<IStudentRepo> _mockRepo;
        private readonly StudentService _studentService;

        public StudentServiceTest()
        {
            _mockRepo = new Mock<IStudentRepo>();
            _studentService = new StudentService(_mockRepo.Object);
        }

        [Fact]
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

            _mockRepo.Setup(repo => repo.AddStudent(It.IsAny<Student>())).ReturnsAsync(student);

            // Act
            var result = await _studentService.AddStudent(student);

            // Assert
            _mockRepo.Verify(repo => repo.AddStudent(It.Is<Student>(s => s == student)), Times.Once);
            Assert.Equal(student, result);
        }

        [Fact]
        public async Task GetAllStudents_WhenCalled_ReturnListOfStudents()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student
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
                },
                new Student
                {
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

            // Act
            var result = await _studentService.GetAllStudents();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Maxx", result.First().FirstName);
            Assert.Equal("1234567890", result.Last().Phone);
            Assert.Equal(Gender.MALE, result.Last().Gender);
        }

        [Fact]
        public async Task GetAllStudents_WhenError_ReturnEmptyList()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllStudents()).ReturnsAsync((IEnumerable<Student>)null);

            // Act
            var result = await _studentService.GetAllStudents();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void CalculateAge_InvalidBirthDate_ReturnIncorrectAge()
        {
            // Arrange
            var birthDate = new DateTime(2000, 10, 23);
            var expectedAge = 27;

            // Act
            var result = _studentService.CalculateAge(birthDate);

            // Assert
            Assert.NotEqual(expectedAge, result);
        }

        [Fact]
        public void CalculateAge_ValidBirthDate_ReturnCorrectAge()
        {
            // Arrange
            var birthDate = new DateTime(2000, 1, 1);
            var expectedAge = 24;

            // Act
            var result = _studentService.CalculateAge(birthDate);

            // Assert
            Assert.Equal(expectedAge, result);
        }

        [Fact]
        public async Task DeleteStudent_WithValidStudentId_ShouldReturnTrue()
        {
            // Arrange
            int studentId = 100;

            _mockRepo.Setup(repo => repo.DeleteStudent(studentId)).ReturnsAsync(true);

            // Act
            var result = await _studentService.DeleteStudent(studentId);

            // Assert
            _mockRepo.Verify(repo => repo.DeleteStudent(studentId), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteStudent_WithInvalidStudentId_ShouldReturnFalse()
        {
            // Arrange
            int studentId = 100;

            _mockRepo.Setup(repo => repo.DeleteStudent(studentId)).ReturnsAsync(false);

            // Act
            var result = await _studentService.DeleteStudent(studentId);

            // Assert
            _mockRepo.Verify(repo => repo.DeleteStudent(studentId), Times.Once);
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateDetails_WithValidData_ShouldReturnTrue()
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

            _mockRepo.Setup(repo => repo.UpdateDetails(studentId, studentToUpdate)).ReturnsAsync(true);

            // Act
            var result = await _studentService.UpdateDetails(studentId, studentToUpdate);

            // Assert
            _mockRepo.Verify(repo => repo.UpdateDetails(studentId, It.Is<Student>(s => s == studentToUpdate)), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateDetails_WithInvalidStudentId_ShouldReturnFalse()
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

            _mockRepo.Setup(repo => repo.UpdateDetails(studentId, studentToUpdate)).ReturnsAsync(false);

            // Act
            var result = await _studentService.UpdateDetails(studentId, studentToUpdate);

            // Assert
            _mockRepo.Verify(repo => repo.UpdateDetails(studentId, It.IsAny<Student>()), Times.Once);
            Assert.False(result);
        }

        [Fact]
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

            _mockRepo.Setup(repo => repo.GetStudentById(studentId)).ReturnsAsync(student);

            // Act
            var result = await _studentService.GetStudentById(studentId);

            // Assert
            Assert.Equal(student, result);
        }

        [Fact]
        public async Task GetStudentById_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var studentId = 1;

            _mockRepo.Setup(repo => repo.GetStudentById(studentId)).ReturnsAsync((Student)null);

            // Act
            var result = await _studentService.GetStudentById(studentId);

            // Assert
            Assert.Null(result);
        }
    }

}
