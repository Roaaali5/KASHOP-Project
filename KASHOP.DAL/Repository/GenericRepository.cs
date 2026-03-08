using KASHOP.DAL.Data;
using KASHOP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public class GenericRepository<T> :IGenericRepository<T> where T :class
    {
        private ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<T> CareateAsync(T item)
        {
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(T Entity)
        {
            _context.Remove(Entity);
           var Affected= await _context.SaveChangesAsync();
            return Affected > 0;
        }

        public async Task<List<T>> GetAllAsync(string[]? includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
            {
                foreach(var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.ToListAsync();

        }
        public async Task<T?> GetOne( Expression<Func<T,bool>> filter,string[]? includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstOrDefaultAsync(filter);

        }
    }
}
