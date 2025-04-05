using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public class ChiTietDonDatHangRepository : IChiTietDonDatHangRepository
    {
        private readonly ApplicationDbContext _context;

        public ChiTietDonDatHangRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChiTietDonDatHang>> GetAllAsync()
        {
            return await _context.ChiTietDonDatHangs
                .Include(ct => ct.DonDatHang)
                .Include(ct => ct.Xe) // Changed from SanPham to Xe
                .ToListAsync();
        }

        public async Task<ChiTietDonDatHang> GetByIdAsync(int id)
        {
            return await _context.ChiTietDonDatHangs
                .Include(ct => ct.DonDatHang)
                .Include(ct => ct.Xe) // Changed from SanPham to Xe
                .FirstOrDefaultAsync(ct => ct.Id == id);
        }

        public async Task AddAsync(ChiTietDonDatHang chiTietDonDatHang)
        {
            await _context.ChiTietDonDatHangs.AddAsync(chiTietDonDatHang);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChiTietDonDatHang chiTietDonDatHang)
        {
            _context.ChiTietDonDatHangs.Update(chiTietDonDatHang);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var chiTietDonDatHang = await _context.ChiTietDonDatHangs.FindAsync(id);
            if (chiTietDonDatHang != null)
            {
                _context.ChiTietDonDatHangs.Remove(chiTietDonDatHang);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ChiTietDonDatHangs.AnyAsync(ct => ct.Id == id);
        }
    }
}
