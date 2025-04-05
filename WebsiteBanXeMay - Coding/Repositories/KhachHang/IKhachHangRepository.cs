using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public interface IKhachHangRepository
    {
        Task<IEnumerable<KhachHang>> GetAllAsync();
        Task<KhachHang> GetByIdAsync(int id);
        Task AddAsync(KhachHang khachHang);
        Task UpdateAsync(KhachHang khachHang);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
