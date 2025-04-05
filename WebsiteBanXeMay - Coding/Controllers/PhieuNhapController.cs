using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Controllers
{
    [Authorize] // ✅ Chỉ cho phép user đăng nhập
    public class PhieuNhapController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PhieuNhapController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 📌 1. Danh sách phiếu nhập
        public async Task<IActionResult> Index()
        {
            var phieuNhapList = await _context.PhieuNhaps
                .AsNoTracking()
                .Include(p => p.NhaCungCap)  // Bỏ Include(NhanVien) nếu lỗi
                .ToListAsync();

            return View(phieuNhapList);
        }

        // 📌 2. Chi tiết phiếu nhập
        public async Task<IActionResult> Details(int id)
        {
            var phieuNhap = await _context.PhieuNhaps
                .AsNoTracking()
                .Include(p => p.NhaCungCap)
                .Include(p => p.NhanVien)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (phieuNhap == null) return NotFound();
            return View(phieuNhap);
        }

        // 📌 3. Form thêm phiếu nhập (GET)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add()
        {
            await LoadDropdownData();
            return View(new PhieuNhap()); // ✅ Tránh lỗi null khi load form
        }

        // 📌 4. Xử lý thêm phiếu nhập (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(PhieuNhap phieuNhap)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownData();
                return View(phieuNhap);
            }

            _context.Add(phieuNhap);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Thêm phiếu nhập thành công!";
            return RedirectToAction(nameof(Index));
        }

        // 📌 5. Form chỉnh sửa phiếu nhập (GET)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var phieuNhap = await _context.PhieuNhaps
                .Include(p => p.NhaCungCap)
                .Include(p => p.NhanVien)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (phieuNhap == null)
            {
                return NotFound();
            }

            await LoadDropdownData(); // Đảm bảo các dữ liệu thả xuống được nạp
            return View(phieuNhap);
        }

        // 📌 6. Xử lý chỉnh sửa phiếu nhập (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, PhieuNhap phieuNhap)
        {
            if (id != phieuNhap.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phieuNhap); // Cập nhật phiếu nhập vào cơ sở dữ liệu
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật phiếu nhập thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.PhieuNhaps.Any(p => p.Id == phieuNhap.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            await LoadDropdownData(); // Nếu model không hợp lệ, nạp lại dữ liệu
            return View(phieuNhap);
        }

        // 📌 7. Xóa phiếu nhập (GET)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var phieuNhap = await _context.PhieuNhaps
                .AsNoTracking()
                .Include(p => p.NhaCungCap)
                .Include(p => p.NhanVien)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (phieuNhap == null)
            {
                TempData["Error"] = "Phiếu nhập không tồn tại!";
                return RedirectToAction(nameof(Index));
            }

            return View(phieuNhap);
        }

        // 📌 8. Xác nhận xóa phiếu nhập (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phieuNhap = await _context.PhieuNhaps.FindAsync(id);
            if (phieuNhap == null)
            {
                TempData["Error"] = "Phiếu nhập không tồn tại hoặc đã bị xóa!";
                return RedirectToAction(nameof(Index));
            }

            _context.PhieuNhaps.Remove(phieuNhap);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Xóa phiếu nhập thành công!";
            return RedirectToAction(nameof(Index));
        }

        // ✅ Load danh sách Nhà Cung Cấp & Nhân Viên
        private async Task LoadDropdownData()
        {
            ViewBag.NhaCungCapList = await _context.NhaCungCaps
                .AsNoTracking()
                .Select(nc => new SelectListItem
                {
                    Value = nc.Id.ToString(),
                    Text = nc.TenNhaCungCap
                }).ToListAsync();

            ViewBag.NhanVienList = await _userManager.Users
                .AsNoTracking()
                .Select(nv => new SelectListItem
                {
                    Value = nv.Id,
                    Text = nv.FullName
                }).ToListAsync();
        }

        // ✅ Kiểm tra phiếu nhập tồn tại
        private bool PhieuNhapExists(int id)
        {
            return _context.PhieuNhaps.Any(p => p.Id == id);
        }
    }
}
