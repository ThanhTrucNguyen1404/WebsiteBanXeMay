using System.Security.Claims;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int GetKhachHangId(ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return 0;

            var khachHang = _context.KhachHangs.FirstOrDefault(k => k.UserId == userId);
            return khachHang?.Id ?? 0;
        }
    }
}
