using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public interface INhaSanXuatRepository
    {
        Task<IEnumerable<NhaSanXuat>> GetAllAsync();
        Task<NhaSanXuat> GetByIdAsync(int id);
        Task AddAsync(NhaSanXuat nhaSanXuat);
        Task UpdateAsync(NhaSanXuat nhaSanXuat);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
