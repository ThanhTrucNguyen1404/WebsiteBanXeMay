using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public interface IDonDatHangRepository
    {
        Task<IEnumerable<DonDatHang>> GetAllAsync();
        Task<DonDatHang> GetByIdAsync(int id);
        Task AddAsync(DonDatHang donDatHang);
        Task UpdateAsync(DonDatHang donDatHang);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
