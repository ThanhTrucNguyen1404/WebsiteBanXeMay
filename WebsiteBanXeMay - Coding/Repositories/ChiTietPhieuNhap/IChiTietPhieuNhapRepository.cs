using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public interface IChiTietPhieuNhapRepository
    {
        Task<IEnumerable<ChiTietPhieuNhap>> GetAllAsync();
        Task<ChiTietPhieuNhap> GetByIdAsync(int id);
        Task AddAsync(ChiTietPhieuNhap chiTietPhieuNhap);
        Task UpdateAsync(ChiTietPhieuNhap chiTietPhieuNhap);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
