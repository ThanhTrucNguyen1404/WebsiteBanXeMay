using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public interface ILoaiSanPhamRepository
    {
        Task<IEnumerable<LoaiSanPham>> GetAllAsync();
        Task<LoaiSanPham> GetByIdAsync(int id);
        Task AddAsync(LoaiSanPham loaiSanPham);
        Task UpdateAsync(LoaiSanPham loaiSanPham);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
