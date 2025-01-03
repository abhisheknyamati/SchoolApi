

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Core.Business.Repositories.Interface;
using SchoolProject.StudentModule.Business.Data;
using SchoolProject.StudentModule.Business.Pagination;

namespace SchoolProject.Core.Business.Repositories
{
    public class GenericReadRepository<T> : IGenericReadRepository<T> where T : class
    {
        private readonly StudentModuleReadDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericReadRepository(StudentModuleReadDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T?> IsDuplicateEmailAsync(Expression<Func<T, bool>> emailPredicate)
        {
            return await _dbSet.FirstOrDefaultAsync(emailPredicate);
        }

    }
}