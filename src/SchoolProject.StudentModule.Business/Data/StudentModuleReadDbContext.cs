
using Microsoft.EntityFrameworkCore;
using SchoolProject.StudentModule.Business.Models;

namespace SchoolProject.StudentModule.Business.Data
{
    public class StudentModuleReadDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public StudentModuleReadDbContext(DbContextOptions<StudentModuleReadDbContext> options) : base(options)
        {
        }
    }
}