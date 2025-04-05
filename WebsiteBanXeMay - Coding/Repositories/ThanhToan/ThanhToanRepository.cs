using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsiteBanXeMay___Coding.Models;
using WebsiteBanXeMay___Coding.Repositories;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public class ThanhToanRepository : IThanhToanRepository
    {
        private readonly ApplicationDbContext _context;

        public ThanhToanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ThanhToan>> GetAllAsync()
        {
            return await _context.ThanhToans.Include(t => t.DonDatHang).ToListAsync();
        }

        public async Task<ThanhToan> GetByIdAsync(int id)
        {
            return await _context.ThanhToans.Include(t => t.DonDatHang)
                                            .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(ThanhToan thanhToan)
        {
            _context.ThanhToans.Add(thanhToan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ThanhToan thanhToan)
        {
            _context.ThanhToans.Update(thanhToan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var thanhToan = await _context.ThanhToans.FindAsync(id);
            if (thanhToan != null)
            {
                _context.ThanhToans.Remove(thanhToan);
                await _context.SaveChangesAsync();
            }
        }
    }
}
