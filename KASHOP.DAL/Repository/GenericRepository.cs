using KASHOP.DAL.Data;
using KASHOP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<T>> GetAllAsync()
        {
            //Include(c => c.Translations).
            return await _context.Set<T>().ToListAsync();

        }
    }
}
