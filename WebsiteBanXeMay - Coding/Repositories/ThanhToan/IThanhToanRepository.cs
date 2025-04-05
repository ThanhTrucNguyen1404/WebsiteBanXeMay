using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public interface IThanhToanRepository
    {
        Task<List<ThanhToan>> GetAllAsync();
        Task<ThanhToan> GetByIdAsync(int id);
        Task AddAsync(ThanhToan thanhToan);
        Task UpdateAsync(ThanhToan thanhToan);
        Task DeleteAsync(int id);
    }
}
