using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Helpers;
using WebsiteBanXeMay___Coding.Models;
using WebsiteBanXeMay___Coding.Repositories; // Thêm namespace repository

namespace WebsiteBanXeMay___Coding.Controllers
{
    public class ThanhVienController : Controller
    {
        private readonly IThanhVienRepository _thanhVienRepository;
        private readonly ApplicationDbContext _context; // Add this line

        public ThanhVienController(IThanhVienRepository thanhVienRepository, ApplicationDbContext context) // Modify constructor
        {
            _thanhVienRepository = thanhVienRepository;
            _context = context; // Initialize _context
        }

        public async Task<IActionResult> Index()
        {
            var thanhViens = await _thanhVienRepository.GetAllAsync();
            return View(thanhViens);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var thanhVien = await _thanhVienRepository.GetByIdAsync(id.Value);
            if (thanhVien == null) return NotFound();

            return View(thanhVien);
        }

        public IActionResult Create()
        {
            // Đây có thể lấy danh sách từ repository, nếu cần thiết
            ViewData["LoaiThanhVienId"] = new SelectList(_context.LoaiThanhViens, "Id", "TenLoai");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HoTen,Email,MatKhau,SoDienThoai,DiaChi,LoaiThanhVienId")] ThanhVien thanhVien)
        {
            if (await _thanhVienRepository.EmailExistsAsync(thanhVien.Email))
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng.");
            }

            if (ModelState.IsValid)
            {
                // Hash mật khẩu trước khi lưu
                thanhVien.MatKhau = PasswordHelper.HashPassword(thanhVien.MatKhau);

                await _thanhVienRepository.AddAsync(thanhVien);
                TempData["SuccessMessage"] = "Thêm thành viên thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["LoaiThanhVienId"] = new SelectList(_context.LoaiThanhViens, "Id", "TenLoai", thanhVien.LoaiThanhVienId);
            return View(thanhVien);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var thanhVien = await _thanhVienRepository.GetByIdAsync(id.Value);
            if (thanhVien == null) return NotFound();

            ViewData["LoaiThanhVienId"] = new SelectList(_context.LoaiThanhViens, "Id", "TenLoai", thanhVien.LoaiThanhVienId);
            return View(thanhVien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HoTen,Email,SoDienThoai,DiaChi,LoaiThanhVienId")] ThanhVien thanhVien)
        {
            if (id != thanhVien.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _thanhVienRepository.UpdateAsync(thanhVien);
                    TempData["SuccessMessage"] = "Cập nhật thành viên thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _thanhVienRepository.GetByIdAsync(thanhVien.Id) == null)
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["LoaiThanhVienId"] = new SelectList(_context.LoaiThanhViens, "Id", "TenLoai", thanhVien.LoaiThanhVienId);
            return View(thanhVien);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var thanhVien = await _thanhVienRepository.GetByIdAsync(id.Value);
            if (thanhVien == null) return NotFound();

            // Kiểm tra nếu thành viên có đơn hàng thì không được xóa
            bool hasOrders = await _context.DonDatHangs.AnyAsync(d => d.KhachHangId == id);
            if (hasOrders)
            {
                TempData["ErrorMessage"] = "Không thể xóa thành viên có đơn hàng!";
                return RedirectToAction(nameof(Index));
            }

            return View(thanhVien);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var thanhVien = await _thanhVienRepository.GetByIdAsync(id);
            if (thanhVien != null)
            {
                // Kiểm tra lần nữa để đảm bảo thành viên không có đơn hàng
                bool hasOrders = await _context.DonDatHangs.AnyAsync(d => d.KhachHangId == id);
                if (hasOrders)
                {
                    TempData["ErrorMessage"] = "Không thể xóa thành viên có đơn hàng!";
                    return RedirectToAction(nameof(Index));
                }

                await _thanhVienRepository.DeleteAsync(id);
                TempData["SuccessMessage"] = "Xóa thành viên thành công!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
