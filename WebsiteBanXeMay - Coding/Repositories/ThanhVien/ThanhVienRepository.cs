using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public class ThanhVienRepository : IThanhVienRepository
    {
        private readonly ApplicationDbContext _context;

        public ThanhVienRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ThanhVien>> GetAllAsync()
        {
            return await _context.ThanhViens.ToListAsync();
        }

        public async Task<ThanhVien> GetByIdAsync(int id)
        {
            return await _context.ThanhViens
                .FirstOrDefaultAsync(tv => tv.Id == id);
        }

        public async Task AddAsync(ThanhVien thanhVien)
        {
            await _context.ThanhViens.AddAsync(thanhVien);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ThanhVien thanhVien)
        {
            _context.ThanhViens.Update(thanhVien);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var thanhVien = await _context.ThanhViens.FindAsync(id);
            if (thanhVien != null)
            {
                _context.ThanhViens.Remove(thanhVien);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ThanhViens.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.ThanhViens.AnyAsync(tv => tv.Email == email);
        }
    }
}
