using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public interface IThanhVienRepository
    {
        Task<IEnumerable<ThanhVien>> GetAllAsync();
        Task<ThanhVien> GetByIdAsync(int id);
        Task AddAsync(ThanhVien thanhVien);
        Task UpdateAsync(ThanhVien thanhVien);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email); // Add this line
    }

}
