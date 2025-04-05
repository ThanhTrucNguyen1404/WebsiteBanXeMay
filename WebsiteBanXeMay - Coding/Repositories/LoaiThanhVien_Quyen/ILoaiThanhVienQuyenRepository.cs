using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public interface ILoaiThanhVienQuyenRepository
    {
        Task<IEnumerable<LoaiThanhVien_Quyen>> GetAllAsync();
        Task<LoaiThanhVien_Quyen> GetByIdAsync(int id);
        Task AddAsync(LoaiThanhVien_Quyen loaiThanhVienQuyen);
        Task UpdateAsync(LoaiThanhVien_Quyen loaiThanhVienQuyen);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
