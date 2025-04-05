using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public interface IXeRepository
    {
        Task<IEnumerable<Xe>> GetAllAsync();
        Task<Xe> GetByIdAsync(int id);
        Task AddAsync(Xe xe);
        Task UpdateAsync(Xe xe);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
