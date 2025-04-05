using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public class PhieuNhapRepository : IPhieuNhapRepository
    {
        private readonly ApplicationDbContext _context;

        public PhieuNhapRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PhieuNhap>> GetAllAsync()
        {
            return await _context.PhieuNhaps.ToListAsync();
        }

        public async Task<PhieuNhap> GetByIdAsync(int id)
        {
            return await _context.PhieuNhaps
                .FirstOrDefaultAsync(pn => pn.Id == id);
        }

        public async Task AddAsync(PhieuNhap phieuNhap)
        {
            await _context.PhieuNhaps.AddAsync(phieuNhap);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PhieuNhap phieuNhap)
        {
            _context.PhieuNhaps.Update(phieuNhap);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var phieuNhap = await _context.PhieuNhaps.FindAsync(id);
            if (phieuNhap != null)
            {
                _context.PhieuNhaps.Remove(phieuNhap);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.PhieuNhaps.AnyAsync(e => e.Id == id);
        }
    }
}
