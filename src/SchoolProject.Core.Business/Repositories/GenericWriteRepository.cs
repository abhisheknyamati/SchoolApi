

using Microsoft.EntityFrameworkCore;
using SchoolProject.Core.Business.Repositories.Interface;
using SchoolProject.StudentModule.Business.Data;

namespace SchoolProject.Core.Business.Repositories
{
    public class GenericWriteRepository<T> : IGenericWriteRepository<T> where T : class
    {
        private readonly StudentModuleWriteDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericWriteRepository(StudentModuleWriteDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity, T updatedEntity)
        {
            _context.Entry(entity).CurrentValues.SetValues(updatedEntity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T?> SoftDeleteAsync(T entity)
        {
            var property = typeof(T).GetProperty("IsActive");
            if (property == null || property.PropertyType != typeof(bool))
            {
                throw new InvalidOperationException("The entity does not have an 'IsActive' property of type bool.");
            }
            var propertyValue = property.GetValue(entity);
            if (propertyValue != null && propertyValue.Equals(true))
            {
                property.SetValue(entity, false);
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            return null;
        }
    }
}