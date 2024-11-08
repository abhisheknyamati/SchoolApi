using Microsoft.EntityFrameworkCore;
using SchoolProject.UserModule.Business.Data;
using SchoolProject.UserModule.Business.Models;
using SchoolProject.UserModule.Business.Repositories.Interfaces;

namespace SchoolProject.UserModule.Business.Repositories
{
    public class AuthRepo(UserModuleDbContext context) : IAuthRepo
    {
        private readonly UserModuleDbContext _context = context;

        public async Task<User> ValidateUser(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(i => i.Email == email && i.Password == password) ?? new User();
        }
    }
}