using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public interface IPhieuNhapRepository
    {
        Task<IEnumerable<PhieuNhap>> GetAllAsync();
        Task<PhieuNhap> GetByIdAsync(int id);
        Task AddAsync(PhieuNhap phieuNhap);
        Task UpdateAsync(PhieuNhap phieuNhap);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
