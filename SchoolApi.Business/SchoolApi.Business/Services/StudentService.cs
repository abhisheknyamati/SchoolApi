using SchoolApi.Business.Models;
using SchoolApi.Business.Repositories;
using SchoolApi.Business.Pagination;

namespace SchoolApi.Business.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepo _repo;
        public StudentService(IStudentRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            var student = await _repo.GetAllStudents();
            return student ?? new List<Student>();
        }

        public async Task<Student> AddStudent(Student student)
        {
            return await _repo.AddStudent(student);
        }

        public async Task<bool> DeleteStudent(int studentId)
        {
            return await _repo.DeleteStudent(studentId);
        }

        public async Task<bool> UpdateDetails(int id, Student student)
        {
            return await _repo.UpdateDetails(id, student);
        }

        public async Task<PagedResponse<Student>> GetStudents(int pageNumber, int pageSize, string searchTerm)
        {
            return await _repo.GetStudents(pageNumber, pageSize, searchTerm);
        }

        public int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }

        public async Task<Student> GetStudentById(int id)
        {
            return await _repo.GetStudentById(id);
        }
    }
}

