using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public class XeRepository : IXeRepository
    {
        private readonly ApplicationDbContext _context;

        public XeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Xe>> GetAllAsync()
        {
            return await _context.Xes.ToListAsync();
        }

        public async Task<Xe> GetByIdAsync(int id)
        {
            return await _context.Xes
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Xe xe)
        {
            await _context.Xes.AddAsync(xe);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Xe xe)
        {
            _context.Xes.Update(xe);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var xe = await _context.Xes.FindAsync(id);
            if (xe != null)
            {
                _context.Xes.Remove(xe);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Xes.AnyAsync(e => e.Id == id);
        }
    }
}
