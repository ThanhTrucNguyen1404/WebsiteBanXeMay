using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public interface ILoaiThanhVienRepository
    {
        Task<IEnumerable<LoaiThanhVien>> GetAllAsync();
        Task<LoaiThanhVien> GetByIdAsync(int id);
        Task AddAsync(LoaiThanhVien loaiThanhVien);
        Task UpdateAsync(LoaiThanhVien loaiThanhVien);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
