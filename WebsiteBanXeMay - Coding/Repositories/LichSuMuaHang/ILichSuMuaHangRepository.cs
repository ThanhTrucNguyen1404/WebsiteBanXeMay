using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public interface ILichSuMuaHangRepository
    {
        Task<IEnumerable<LichSuMuaHang>> GetAllAsync();
        Task<LichSuMuaHang> GetByIdAsync(int id);
        Task<IEnumerable<LichSuMuaHang>> GetByKhachHangIdAsync(int khachHangId);
        Task AddAsync(LichSuMuaHang lichSuMuaHang);
        Task UpdateAsync(LichSuMuaHang lichSuMuaHang);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
