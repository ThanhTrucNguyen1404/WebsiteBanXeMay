using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public class LoaiThanhVienQuyenRepository : ILoaiThanhVienQuyenRepository
    {
        private readonly ApplicationDbContext _context;

        public LoaiThanhVienQuyenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LoaiThanhVien_Quyen>> GetAllAsync()
        {
            return await _context.LoaiThanhVien_Quyens
                .Include(lq => lq.LoaiThanhVien)
                .Include(lq => lq.Quyen)
                .ToListAsync();
        }

        public async Task<LoaiThanhVien_Quyen> GetByIdAsync(int loaiThanhVienId, int quyenId)
        {
            return await _context.LoaiThanhVien_Quyens
                .Include(lq => lq.LoaiThanhVien)
                .Include(lq => lq.Quyen)
                .FirstOrDefaultAsync(lq => lq.LoaiThanhVienId == loaiThanhVienId && lq.QuyenId == quyenId);
        }

        public async Task AddAsync(LoaiThanhVien_Quyen loaiThanhVienQuyen)
        {
            await _context.LoaiThanhVien_Quyens.AddAsync(loaiThanhVienQuyen);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LoaiThanhVien_Quyen loaiThanhVienQuyen)
        {
            _context.LoaiThanhVien_Quyens.Update(loaiThanhVienQuyen);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var loaiThanhVienQuyen = await _context.LoaiThanhVien_Quyens.FindAsync(id);
            if (loaiThanhVienQuyen != null)
            {
                _context.LoaiThanhVien_Quyens.Remove(loaiThanhVienQuyen);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int loaiThanhVienId, int quyenId)
        {
            return await _context.LoaiThanhVien_Quyens.AnyAsync(e => e.LoaiThanhVienId == loaiThanhVienId && e.QuyenId == quyenId);
        }

        // Implementing the missing interface member
        public async Task<LoaiThanhVien_Quyen> GetByIdAsync(int id)
        {
            return await _context.LoaiThanhVien_Quyens.FindAsync(id);
        }

        // Implementing the missing interface member
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.LoaiThanhVien_Quyens.AnyAsync(e => e.LoaiThanhVienId == id || e.QuyenId == id);
        }
    }
}
