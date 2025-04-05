using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public interface IGioHangRepository
    {
        Task<List<GioHangItem>> GetByKhachHangIdAsync(int khachHangId);
        Task UpdateRangeAsync(List<GioHangItem> gioHangItems);
    }
}
