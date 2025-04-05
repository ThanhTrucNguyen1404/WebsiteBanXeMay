using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public class ChiTietPhieuNhapRepository : IChiTietPhieuNhapRepository
    {
        private readonly ApplicationDbContext _context;

        public ChiTietPhieuNhapRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChiTietPhieuNhap>> GetAllAsync()
        {
            return await _context.ChiTietPhieuNhaps
                .Include(ct => ct.PhieuNhap)
                .Include(ct => ct.Xe) // Fixed line
                .ToListAsync();
        }

        public async Task<ChiTietPhieuNhap> GetByIdAsync(int id)
        {
            return await _context.ChiTietPhieuNhaps
                .Include(ct => ct.PhieuNhap)
                .Include(ct => ct.Xe) // Fixed line
                .FirstOrDefaultAsync(ct => ct.Id == id);
        }

        public async Task AddAsync(ChiTietPhieuNhap chiTietPhieuNhap)
        {
            await _context.ChiTietPhieuNhaps.AddAsync(chiTietPhieuNhap);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChiTietPhieuNhap chiTietPhieuNhap)
        {
            _context.ChiTietPhieuNhaps.Update(chiTietPhieuNhap);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var chiTietPhieuNhap = await _context.ChiTietPhieuNhaps.FindAsync(id);
            if (chiTietPhieuNhap != null)
            {
                _context.ChiTietPhieuNhaps.Remove(chiTietPhieuNhap);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ChiTietPhieuNhaps.AnyAsync(ct => ct.Id == id);
        }
    }
}
