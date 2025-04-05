using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public class NhanVienRepository : INhanVienRepository
    {
        private readonly ApplicationDbContext _context;

        public NhanVienRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NhanVien>> GetAllAsync()
        {
            return await _context.Users.Include(nv => nv.NhanVien)
                                       .Select(nv => nv.NhanVien)
                                       .ToListAsync();
        }


        public async Task<NhanVien?> GetByIdAsync(int id)
        {
            return await _context.Users.Include(nv => nv.NhanVien)
                                       .Where(nv => nv.NhanVien.Id == id)
                                       .Select(nv => nv.NhanVien)
                                       .FirstOrDefaultAsync();
        }

        public async Task AddAsync(NhanVien nhanVien)
        {
            _context.Users.Add(nhanVien.User);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NhanVien nhanVien)
        {
            _context.Users.Update(nhanVien.User);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var nhanVien = await GetByIdAsync(id);
            if (nhanVien != null)
            {
                _context.Users.Remove(nhanVien.User);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<NhanVien?> GetByUserIdAsync(string userId)
        {
            return await _context.Users.Include(nv => nv.NhanVien)
                                       .Where(nv => nv.Id == userId)
                                       .Select(nv => nv.NhanVien)
                                       .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(nv => nv.NhanVien.Id == id);
        }
    }
}
