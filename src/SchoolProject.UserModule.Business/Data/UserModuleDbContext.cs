
using Microsoft.EntityFrameworkCore;
using SchoolProject.UserModule.Business.Models;

namespace SchoolProject.UserModule.Business.Data
{
    public class UserModuleDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public UserModuleDbContext(DbContextOptions<UserModuleDbContext> options) : base(options) { }
    }
}
