using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public class LoaiThanhVienRepository : ILoaiThanhVienRepository
    {
        private readonly ApplicationDbContext _context;

        public LoaiThanhVienRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LoaiThanhVien>> GetAllAsync()
        {
            return await _context.LoaiThanhViens.ToListAsync();
        }

        public async Task<LoaiThanhVien> GetByIdAsync(int id)
        {
            return await _context.LoaiThanhViens.FindAsync(id);
        }

        public async Task AddAsync(LoaiThanhVien loaiThanhVien)
        {
            await _context.LoaiThanhViens.AddAsync(loaiThanhVien);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LoaiThanhVien loaiThanhVien)
        {
            _context.LoaiThanhViens.Update(loaiThanhVien);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var loaiThanhVien = await _context.LoaiThanhViens.FindAsync(id);
            if (loaiThanhVien != null)
            {
                _context.LoaiThanhViens.Remove(loaiThanhVien);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.LoaiThanhViens.AnyAsync(e => e.Id == id);
        }
    }
}
