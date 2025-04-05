using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public class LichSuMuaHangRepository : ILichSuMuaHangRepository
    {
        private readonly ApplicationDbContext _context;

        public LichSuMuaHangRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LichSuMuaHang>> GetAllAsync()
        {
            return await _context.LichSuMuaHangs
                .Include(l => l.KhachHang)
                .Include(l => l.GioHangItems)
                .ThenInclude(g => g.SanPham)
                .ToListAsync();
        }

        public async Task<LichSuMuaHang> GetByIdAsync(int id)
        {
            return await _context.LichSuMuaHangs
                .Include(l => l.KhachHang)
                .Include(l => l.GioHangItems)
                .ThenInclude(g => g.SanPham)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<LichSuMuaHang>> GetByKhachHangIdAsync(int khachHangId)
        {
            return await _context.LichSuMuaHangs
                .Where(l => l.KhachHangId == khachHangId)
                .Include(l => l.GioHangItems)
                .ThenInclude(g => g.SanPham)
                .ToListAsync();
        }

        public async Task AddAsync(LichSuMuaHang lichSuMuaHang)
        {
            await _context.LichSuMuaHangs.AddAsync(lichSuMuaHang);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LichSuMuaHang lichSuMuaHang)
        {
            _context.LichSuMuaHangs.Update(lichSuMuaHang);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var lichSuMuaHang = await _context.LichSuMuaHangs.FindAsync(id);
            if (lichSuMuaHang != null)
            {
                _context.LichSuMuaHangs.Remove(lichSuMuaHang);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
