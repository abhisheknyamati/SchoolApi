using SchoolApi.Business.Models;
using SchoolApi.Business.Pagination;

namespace SchoolApi.Business.Repositories
{
    public interface IStudentRepo
    {
        Task<IEnumerable<Student>> GetAllStudents();
        Task<Student> AddStudent(Student student);
        Task<bool> DeleteStudent(int studentId);
        Task<bool> UpdateDetails(int id, Student student);
        Task<PagedResponse<Student>> GetStudents(int pageNumber, int pageSize, string searchTerm);
        Task<Student> GetStudentById(int id);
    }
}
