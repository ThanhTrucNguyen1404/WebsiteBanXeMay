using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public class NhaSanXuatRepository : INhaSanXuatRepository
    {
        private readonly ApplicationDbContext _context;

        public NhaSanXuatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NhaSanXuat>> GetAllAsync()
        {
            return await _context.NhaSanXuats.ToListAsync();
        }

        public async Task<NhaSanXuat> GetByIdAsync(int id)
        {
            return await _context.NhaSanXuats.FirstOrDefaultAsync(nsx => nsx.Id == id);
        }

        public async Task AddAsync(NhaSanXuat nhaSanXuat)
        {
            await _context.NhaSanXuats.AddAsync(nhaSanXuat);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NhaSanXuat nhaSanXuat)
        {
            _context.NhaSanXuats.Update(nhaSanXuat);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var nhaSanXuat = await _context.NhaSanXuats.FindAsync(id);
            if (nhaSanXuat != null)
            {
                _context.NhaSanXuats.Remove(nhaSanXuat);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.NhaSanXuats.AnyAsync(e => e.Id == id);
        }
    }
}
