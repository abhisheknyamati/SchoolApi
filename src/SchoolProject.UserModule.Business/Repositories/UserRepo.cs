using Microsoft.EntityFrameworkCore;
using SchoolProject.UserModule.Business.Data;
using SchoolProject.UserModule.Business.Models;
using SchoolProject.UserModule.Business.Repositories.Interfaces;

namespace SchoolProject.UserModule.Business.Repositories
{
    public class UserRepo(UserModuleDbContext context) : IUserRepo
    {
        private readonly UserModuleDbContext _context = context;

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> DeleteUser(User user)
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
        public async Task<User> GetUserById(int id)
        {
            var admin = await _context.Users.FirstOrDefaultAsync(i => i.Id == id);
            if (admin == null)
            {
                throw new KeyNotFoundException($"Admin with Id {id} not found.");
            }
            return admin;
        }

        public async Task<User> AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}