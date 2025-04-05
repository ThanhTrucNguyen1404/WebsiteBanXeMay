using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public class NhaCungCapRepository : INhaCungCapRepository
    {
        private readonly ApplicationDbContext _context;

        public NhaCungCapRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NhaCungCap>> GetAllAsync()
        {
            return await _context.NhaCungCaps.ToListAsync();
        }

        public async Task<NhaCungCap> GetByIdAsync(int id)
        {
            return await _context.NhaCungCaps.FirstOrDefaultAsync(nc => nc.Id == id);
        }

        public async Task AddAsync(NhaCungCap nhaCungCap)
        {
            await _context.NhaCungCaps.AddAsync(nhaCungCap);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NhaCungCap nhaCungCap)
        {
            _context.NhaCungCaps.Update(nhaCungCap);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var nhaCungCap = await _context.NhaCungCaps.FindAsync(id);
            if (nhaCungCap != null)
            {
                _context.NhaCungCaps.Remove(nhaCungCap);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.NhaCungCaps.AnyAsync(e => e.Id == id);
        }
    }
}
