using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public interface IHinhAnhRepository
    {
        Task<IEnumerable<HinhAnh>> GetAllAsync();
        Task<HinhAnh> GetByIdAsync(int id);
        Task<IEnumerable<HinhAnh>> GetBySanPhamIdAsync(int sanPhamId);
        Task AddAsync(HinhAnh hinhAnh);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}
