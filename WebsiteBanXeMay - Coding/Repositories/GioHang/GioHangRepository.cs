using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public class GioHangRepository : IGioHangRepository
    {
        private readonly ApplicationDbContext _context;

        public GioHangRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GioHangItem>> GetByKhachHangIdAsync(int khachHangId)
        {
            return await _context.GioHangItems
                .Where(x => x.SanPham.NhaCungCapId == khachHangId && x.LichSuMuaHangId == null) // Giỏ hàng chưa thanh toán
                .Include(x => x.SanPham)
                .ToListAsync();
        }

        public async Task UpdateRangeAsync(List<GioHangItem> gioHangItems)
        {
            _context.GioHangItems.UpdateRange(gioHangItems);
            await _context.SaveChangesAsync();
        }
    }
}
