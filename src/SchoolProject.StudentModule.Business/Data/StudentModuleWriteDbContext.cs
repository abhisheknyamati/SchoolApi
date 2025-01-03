using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolProject.StudentModule.Business.Models;

namespace SchoolProject.StudentModule.Business.Data
{
    public class StudentModuleWriteDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public StudentModuleWriteDbContext(DbContextOptions<StudentModuleWriteDbContext> options) : base(options)
        {
        }
    }
}