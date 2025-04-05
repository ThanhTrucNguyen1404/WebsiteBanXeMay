using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public class DonDatHangRepository : IDonDatHangRepository
    {
        private readonly ApplicationDbContext _context;

        public DonDatHangRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DonDatHang>> GetAllAsync()
        {
            return await _context.DonDatHangs
                .Include(d => d.KhachHang)
                .Include(d => d.ChiTietDonDatHangs)
                .ThenInclude(ct => ct.Xe)
                .ToListAsync();
        }

        public async Task<DonDatHang> GetByIdAsync(int id)
        {
            return await _context.DonDatHangs
                .Include(d => d.KhachHang)
                .Include(d => d.ChiTietDonDatHangs)
                .ThenInclude(ct => ct.Xe)
                .FirstOrDefaultAsync(d => d.DonDatHangId == id);
        }

        public async Task AddAsync(DonDatHang donDatHang)
        {
            await _context.DonDatHangs.AddAsync(donDatHang);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DonDatHang donDatHang)
        {
            _context.DonDatHangs.Update(donDatHang);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var donDatHang = await _context.DonDatHangs.FindAsync(id);
            if (donDatHang != null)
            {
                _context.DonDatHangs.Remove(donDatHang);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.DonDatHangs.AnyAsync(d => d.DonDatHangId == id);
        }
    }
}
