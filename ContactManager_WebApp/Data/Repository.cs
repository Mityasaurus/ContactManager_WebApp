using Microsoft.EntityFrameworkCore;
using ContactManager_WebApp.Data.Interfaces;

namespace ContactManager_WebApp.Data
{
    public class Repository<T>(ContactManagerContext context) : IRepository<T> where T : class
    {
        private readonly ContactManagerContext _context = context;

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T?> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<bool> AddAsync(T data)
        {
            try
            {
                _context.Set<T>().Add(data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(T data)
        {
            try
            {
                _context.Entry(data).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var item = await GetAsync(id);
                _context.Set<T>().Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddRangeAsync(IEnumerable<T> data)
        {
            try
            {
                await _context.AddRangeAsync(data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
