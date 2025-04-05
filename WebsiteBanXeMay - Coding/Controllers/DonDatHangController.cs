using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Controllers
{
    public class DonDatHangController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DonDatHangController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 📌 1️⃣ Hiển thị danh sách đơn hàng
        public async Task<IActionResult> Index()
        {
            var donDatHangs = await _context.DonDatHangs
                .Include(d => d.KhachHang)
                .Include(d => d.ChiTietDonDatHangs)
                .ThenInclude(ct => ct.Xe)
                .ToListAsync();
            return View(donDatHangs);
        }

        // 📌 2️⃣ Xem chi tiết đơn hàng
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var donDatHang = await _context.DonDatHangs
                .Include(d => d.KhachHang)
                .Include(d => d.ChiTietDonDatHangs)
                .ThenInclude(ct => ct.Xe)
                .FirstOrDefaultAsync(m => m.DonDatHangId == id);

            if (donDatHang == null) return NotFound();

            return View(donDatHang);
        }

        // 📌 3️⃣ Hiển thị form thêm đơn hàng (GET)
        public IActionResult Add()
        {
            ViewBag.KhachHangList = new SelectList(_context.KhachHangs, "Id", "HoTen");
            return View();
        }

        // 📌 4️⃣ Xử lý thêm đơn hàng (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(DonDatHang donDatHang)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.KhachHangList = new SelectList(_context.KhachHangs, "Id", "HoTen");
                return View(donDatHang);
            }

            try
            {
                donDatHang.NgayDat = DateTime.Now;
                donDatHang.OrderId = Guid.NewGuid().ToString();
                donDatHang.TrangThai = "Chờ xác nhận";

                _context.Add(donDatHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi lưu đơn hàng: " + ex.Message);
            }

            ViewBag.KhachHangList = new SelectList(_context.KhachHangs, "Id", "HoTen");
            return View(donDatHang);
        }

        // 📌 5️⃣ Hiển thị form chỉnh sửa đơn hàng (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var donDatHang = await _context.DonDatHangs.FindAsync(id);
            if (donDatHang == null) return NotFound();

            ViewBag.KhachHangList = new SelectList(_context.KhachHangs, "Id", "HoTen", donDatHang.KhachHangId);
            return View(donDatHang);
        }

        // 📌 6️⃣ Xử lý chỉnh sửa đơn hàng (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DonDatHang donDatHang)
        {
            if (id != donDatHang.DonDatHangId) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.KhachHangList = new SelectList(_context.KhachHangs, "Id", "HoTen", donDatHang.KhachHangId);
                return View(donDatHang);
            }

            try
            {
                _context.Update(donDatHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi cập nhật đơn hàng: " + ex.Message);
            }

            ViewBag.KhachHangList = new SelectList(_context.KhachHangs, "Id", "HoTen", donDatHang.KhachHangId);
            return View(donDatHang);
        }

        // 📌 7️⃣ Xóa đơn hàng
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var donDatHang = await _context.DonDatHangs
                .Include(d => d.KhachHang)
                .FirstOrDefaultAsync(m => m.DonDatHangId == id);

            if (donDatHang == null) return NotFound();

            return View(donDatHang);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donDatHang = await _context.DonDatHangs.FindAsync(id);
            if (donDatHang != null)
            {
                _context.DonDatHangs.Remove(donDatHang);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // 📌 8️⃣ Xử lý đặt hàng từ giỏ hàng (Checkout)
        [HttpPost]
        public async Task<IActionResult> Checkout(int khachHangId, string paymentMethod)
        {
            var gioHangItems = await _context.GioHangItems
                .Where(g => g.LichSuMuaHangId == null && g.Status == "Đang chờ")
                .ToListAsync();

            if (!gioHangItems.Any()) return BadRequest("Giỏ hàng trống.");

            decimal tongTien = gioHangItems.Sum(item => item.ThanhTien);

            var donDatHang = new DonDatHang
            {
                KhachHangId = khachHangId,
                NgayDat = DateTime.Now,
                TongTien = tongTien,
                TrangThai = "Chờ xác nhận",
                OrderId = Guid.NewGuid().ToString(),
                Status = "Pending"
            };

            _context.DonDatHangs.Add(donDatHang);
            await _context.SaveChangesAsync();

            

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Đặt hàng thành công!", OrderId = donDatHang.OrderId });
        }

        // 📌 9️⃣ Xem chi tiết đơn hàng của user
        public async Task<IActionResult> ChiTietDonHang(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var khachHang = await _context.KhachHangs.FirstOrDefaultAsync(k => k.UserId == user.Id);
            if (khachHang == null) return NotFound("Khách hàng không tồn tại.");

            var donHang = await _context.DonDatHangs
                .Include(d => d.ChiTietDonDatHangs)
                .ThenInclude(ct => ct.Xe)
                .FirstOrDefaultAsync(d => d.DonDatHangId == id && d.KhachHangId == khachHang.Id);

            if (donHang == null) return NotFound();

            return View(donHang);
        }
    }
}
