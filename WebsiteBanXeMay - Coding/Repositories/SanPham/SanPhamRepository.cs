using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public class SanPhamRepository : ISanPhamRepository
    {
        private readonly ApplicationDbContext _context;

        public SanPhamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SanPham>> GetAllAsync()
        {
            return await _context.SanPhams.ToListAsync();
        }

        public async Task<SanPham> GetByIdAsync(int id)
        {
            return await _context.SanPhams.FindAsync(id);
        }

        public async Task AddAsync(SanPham sanPham)
        {
            await _context.SanPhams.AddAsync(sanPham);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SanPham sanPham)
        {
            _context.SanPhams.Update(sanPham);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham != null)
            {
                _context.SanPhams.Remove(sanPham);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.SanPhams.AnyAsync(sp => sp.Id == id);
        }
    }
}
