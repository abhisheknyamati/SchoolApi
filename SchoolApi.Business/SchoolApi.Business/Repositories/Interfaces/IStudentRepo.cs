using SchoolApi.Business.Models;
using SchoolApi.Business.Pagination;

namespace SchoolApi.Business.Repositories
{
    public interface IStudentRepo
    {
        Task<IEnumerable<Student>> GetAllStudents();
        Task<Student> AddStudent(Student student);
        Task<bool> DeleteStudent(Student student);
        Task<Student> UpdateDetails(Student student);
        Task<PagedResponse<Student>> GetStudents(int pageNumber, int pageSize, string searchTerm);
        Task<Student> GetStudentById(int id);
    }
}
