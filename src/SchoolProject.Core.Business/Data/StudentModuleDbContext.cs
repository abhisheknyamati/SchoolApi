
using Microsoft.EntityFrameworkCore;
using SchoolProject.Core.Business.Models;

namespace SchoolProject.Core.Business.Data
{
    public class StudentModuleDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public StudentModuleDbContext(DbContextOptions<StudentModuleDbContext> options) : base(options) { }
    }
}
