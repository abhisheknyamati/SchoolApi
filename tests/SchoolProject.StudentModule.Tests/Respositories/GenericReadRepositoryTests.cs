
using Bogus;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Core.Business.Repositories;
using SchoolProject.StudentModule.Business.Data;
using SchoolProject.StudentModule.Business.Models;
using SchoolProject.StudentModule.Business.Models.ENUM;

namespace SchoolProject.StudentModule.Tests.Respositories
{
    [TestClass]
    public class GenericReadRepositoryTests : IAsyncDisposable
    {
        private StudentModuleReadDbContext _context;
        private GenericReadRepository<Student> _repository;

        [TestInitialize]
        public async Task TestInitialize()
        {
            var options = new DbContextOptionsBuilder<StudentModuleReadDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new StudentModuleReadDbContext(options);
            _repository = new GenericReadRepository<Student>(_context);

            // arrange
            var fakeStudents = new Faker<Student>()
                .RuleFor(s => s.Id, f => f.IndexFaker + 1)
                .RuleFor(s => s.FirstName, f => f.Name.FirstName())
                .RuleFor(s => s.LastName, f => f.Name.LastName())
                .RuleFor(s => s.Email, (f, s) => f.Internet.Email(s.FirstName, s.LastName))
                .RuleFor(s => s.Phone, f => $"{f.PickRandom("7", "8", "9")}{f.Random.Number(100000000, 999999999)}")
                .RuleFor(s => s.Address, f => f.Address.FullAddress())
                .RuleFor(s => s.Gender, f => f.PickRandom<Gender>())
                .RuleFor(s => s.BirthDate, f => f.Date.Past(50, DateTime.Now.AddYears(-18)))
                .RuleFor(s => s.IsActive, f => true)
                .Generate(5);

            _context.Students.AddRange(fakeStudents);
            await _context.SaveChangesAsync();
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.AreEqual(5, result.Count());
            Assert.IsTrue(result.All(s => s.IsActive == true));
            Assert.IsTrue(result.All(s => !string.IsNullOrEmpty(s.FirstName) && !string.IsNullOrEmpty(s.LastName) && 
                !string.IsNullOrEmpty(s.Email) && !string.IsNullOrEmpty(s.Phone) && !string.IsNullOrEmpty(s.Address)));
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnCorrectEntity()
        {
            // Arrange
            var expectedStudent = await _context.Students.FirstAsync();

            // Act
            var result = await _repository.GetByIdAsync(expectedStudent.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedStudent.Id, result.Id);
            Assert.AreEqual(expectedStudent.FirstName, result.FirstName);
            Assert.AreEqual(expectedStudent.LastName, result.LastName);
            Assert.AreEqual(expectedStudent.Email, result.Email);
            Assert.IsTrue(result.IsActive);
        }

        [TestMethod]
        public async Task IsDuplicateEmailAsync_ShouldReturnMatchingEntity()
        {
            // Arrange
            var duplicateEmail = (await _context.Students.FirstAsync()).Email;

            // Act
            var result = await _repository.IsDuplicateEmailAsync(s => s.Email == duplicateEmail);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(duplicateEmail, result.Email);
        }

        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
                await _context.Database.EnsureDeletedAsync();
                await _context.DisposeAsync();
            }
        }
    }
}