using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public class LoaiSanPhamRepository : ILoaiSanPhamRepository
    {
        private readonly ApplicationDbContext _context;

        public LoaiSanPhamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LoaiSanPham>> GetAllAsync()
        {
            return await _context.LoaiSanPhams.ToListAsync();
        }

        public async Task<LoaiSanPham> GetByIdAsync(int id)
        {
            return await _context.LoaiSanPhams.FindAsync(id);
        }

        public async Task AddAsync(LoaiSanPham loaiSanPham)
        {
            await _context.LoaiSanPhams.AddAsync(loaiSanPham);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LoaiSanPham loaiSanPham)
        {
            _context.LoaiSanPhams.Update(loaiSanPham);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var loaiSanPham = await _context.LoaiSanPhams.FindAsync(id);
            if (loaiSanPham != null)
            {
                _context.LoaiSanPhams.Remove(loaiSanPham);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.LoaiSanPhams.AnyAsync(e => e.Id == id);
        }
    }
}
