using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public interface IChiTietDonDatHangRepository
    {
        Task<IEnumerable<ChiTietDonDatHang>> GetAllAsync();
        Task<ChiTietDonDatHang> GetByIdAsync(int id);
        Task AddAsync(ChiTietDonDatHang chiTietDonDatHang);
        Task UpdateAsync(ChiTietDonDatHang chiTietDonDatHang);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
