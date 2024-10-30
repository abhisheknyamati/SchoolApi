using SchoolApi.Business.Services;

namespace SchoolApi.Test
{
    public class StudentServiceTest
    {
        private readonly StudentService _studentService;

        public StudentServiceTest()
        {
            _studentService = new StudentService();
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
    }
}