using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public class KhachHangRepository : IKhachHangRepository
    {
        private readonly ApplicationDbContext _context;

        public KhachHangRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<KhachHang>> GetAllAsync()
        {
            return await _context.KhachHangs.ToListAsync();
        }

        public async Task<KhachHang> GetByIdAsync(int id)
        {
            return await _context.KhachHangs.FindAsync(id);
        }

        public async Task AddAsync(KhachHang khachHang)
        {
            await _context.KhachHangs.AddAsync(khachHang);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(KhachHang khachHang)
        {
            _context.KhachHangs.Update(khachHang);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang != null)
            {
                _context.KhachHangs.Remove(khachHang);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.KhachHangs.AnyAsync(kh => kh.Id == id);
        }
    }
}
