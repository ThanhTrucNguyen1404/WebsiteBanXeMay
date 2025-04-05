using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public interface IQuyenRepository
    {
        Task<IEnumerable<Quyen>> GetAllAsync();
        Task<Quyen> GetByIdAsync(int id);
        Task AddAsync(Quyen quyen);
        Task UpdateAsync(Quyen quyen);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
