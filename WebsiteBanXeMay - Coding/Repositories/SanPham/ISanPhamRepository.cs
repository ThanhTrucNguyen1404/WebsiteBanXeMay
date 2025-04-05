using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public interface ISanPhamRepository
    {
        Task<IEnumerable<SanPham>> GetAllAsync();
        Task<SanPham> GetByIdAsync(int id);
        Task AddAsync(SanPham sanPham);
        Task UpdateAsync(SanPham sanPham);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
