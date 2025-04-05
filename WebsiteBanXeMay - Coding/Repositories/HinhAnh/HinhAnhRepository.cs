using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public class HinhAnhRepository : IHinhAnhRepository
    {
        private readonly ApplicationDbContext _context;

        public HinhAnhRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HinhAnh>> GetAllAsync()
        {
            return await _context.Set<HinhAnh>().ToListAsync();
        }

        public async Task<HinhAnh> GetByIdAsync(int id)
        {
            return await _context.Set<HinhAnh>().FindAsync(id);
        }

        public async Task<IEnumerable<HinhAnh>> GetBySanPhamIdAsync(int sanPhamId)
        {
            return await _context.Set<HinhAnh>()
                .Where(h => h.SanPhamId == sanPhamId)
                .ToListAsync();
        }

        public async Task AddAsync(HinhAnh hinhAnh)
        {
            await _context.Set<HinhAnh>().AddAsync(hinhAnh);
            await SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var hinhAnh = await _context.Set<HinhAnh>().FindAsync(id);
            if (hinhAnh != null)
            {
                _context.Set<HinhAnh>().Remove(hinhAnh);
                await SaveAsync();
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
