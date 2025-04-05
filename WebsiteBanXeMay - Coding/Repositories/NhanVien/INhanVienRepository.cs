using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public interface INhanVienRepository
    {
        Task<IEnumerable<NhanVien>> GetAllAsync();   // Lấy danh sách nhân viên
        Task<NhanVien?> GetByIdAsync(int id);        // Lấy nhân viên theo ID
        Task AddAsync(NhanVien nhanVien);            // Thêm nhân viên
        Task UpdateAsync(NhanVien nhanVien);         // Cập nhật nhân viên
        Task DeleteAsync(int id);                    // Xóa nhân viên
        Task<NhanVien?> GetByUserIdAsync(string userId); // Tìm nhân viên theo UserId
        Task<bool> ExistsAsync(int id);              // Kiểm tra nhân viên có tồn tại không
    }
}
