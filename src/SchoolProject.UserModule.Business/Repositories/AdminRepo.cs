using Microsoft.EntityFrameworkCore;
using SchoolProject.UserModule.Business.Data;
using SchoolProject.UserModule.Business.Models;
using SchoolProject.UserModule.Business.Repositories.Interfaces;

namespace SchoolProject.UserModule.Business.Repositories
{
    public class AdminRepo : IAdminRepo
    {
        private readonly UserModuleDbContext _context;
        public AdminRepo(UserModuleDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAdmins()
        {
            return await _context.Users.Where(i => i.IsAdmin).ToListAsync();
        }

        public async Task<bool> DeleteAdmin(User user)
        {
            if (user.IsActive)
            {
                user.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<User> UpdateDetails(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> GetAdminById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<User> AddAdmin(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        
    }
}