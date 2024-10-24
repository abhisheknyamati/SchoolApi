using Microsoft.EntityFrameworkCore;
using SchoolApi.Models;

namespace SchoolApi.Data
{
    public class SchoolAPIDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public SchoolAPIDbContext(DbContextOptions<SchoolAPIDbContext> options) : base(options) { }
    }
}
