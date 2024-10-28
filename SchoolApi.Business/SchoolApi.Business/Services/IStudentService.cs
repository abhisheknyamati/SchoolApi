using SchoolApi.Business.Models;
using SchoolApi.Business.Pagination;

namespace SchoolApi.Business.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudents();
        Task<Student> AddStudent(Student student);
        Task<bool> DeleteStudent(int studentId);
        Task<bool> UpdateDetails(int id, Student studentDto);
        Task<PagedResponse<Student>> GetStudents(int pageNumber, int pageSize, string searchTerm);
        int CalculateAge(DateTime birthDate);
        Task<Student> GetStudentById(int id);
    }
}
