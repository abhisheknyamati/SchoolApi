using Microsoft.AspNetCore.Mvc;
using SchoolApi.Models;
using SchoolApi.Models.DTOs;

namespace SchoolApi.Repositories
{
    public interface IStudentRepo
    {
        Task<IEnumerable<Student>> GetAllStudents();
        Task<IActionResult> AddStudent(Student student);
        Task<IActionResult> DeleteStudent(int studentId);
        Task<IActionResult> UpdateDetails(int id, Student student);
        Task<ActionResult<PagedResponse<Student>>> GetStudents([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string searchTerm);
        Task<ActionResult<Student>> GetStudentById(int id);
    }
}
