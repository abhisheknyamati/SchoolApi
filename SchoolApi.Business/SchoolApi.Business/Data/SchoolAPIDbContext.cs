
using Microsoft.EntityFrameworkCore;
using SchoolApi.Business.Models;

namespace SchoolApi.Business.Data
{
    public class SchoolAPIDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public SchoolAPIDbContext(DbContextOptions<SchoolAPIDbContext> options) : base(options) { }
    }
}
