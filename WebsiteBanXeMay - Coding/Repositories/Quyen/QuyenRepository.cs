using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public class QuyenRepository : IQuyenRepository
    {
        private readonly ApplicationDbContext _context;

        public QuyenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Quyen>> GetAllAsync()
        {
            return await _context.Quyens.ToListAsync();
        }

        public async Task<Quyen> GetByIdAsync(int id)
        {
            return await _context.Quyens
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task AddAsync(Quyen quyen)
        {
            await _context.Quyens.AddAsync(quyen);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Quyen quyen)
        {
            _context.Quyens.Update(quyen);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var quyen = await _context.Quyens.FindAsync(id);
            if (quyen != null)
            {
                _context.Quyens.Remove(quyen);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Quyens.AnyAsync(e => e.Id == id);
        }
    }
}
