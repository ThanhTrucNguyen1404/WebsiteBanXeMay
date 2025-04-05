using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteBanXeMay___Coding.Models;
using WebsiteBanXeMay___Coding.Repositories;
using WebsiteBanXeMay___Coding.Services;

namespace WebsiteBanXeMay___Coding.Controllers
{
    public class LichSuMuaHangController : Controller
    {
        private readonly IGioHangRepository _gioHangRepository;
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;

        public LichSuMuaHangController(
            IGioHangRepository gioHangRepository,
            IUserService userService,
            ApplicationDbContext context)
        {
            _gioHangRepository = gioHangRepository;
            _userService = userService;
            _context = context;
        }

        /// <summary>
        /// ✅ Lấy danh sách sản phẩm đã thanh toán
        /// </summary>
        public async Task<IActionResult> LichSu()
        {
            var khachHangId = _userService.GetKhachHangId(User);

            var lichSuMuaHang = await _context.GioHangItems
                .Where(i => i.OrderId != null && i.LichSuMuaHangId != null)
                .OrderByDescending(i => i.NgayThanhToan)
                .ToListAsync();

            return View("~/Views/GioHang/LichSu.cshtml", lichSuMuaHang);
        }

        /// <summary>
        /// ✅ Xử lý thanh toán và cập nhật OrderId cho giỏ hàng
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ThanhToan()
        {
            var khachHangId = _userService.GetKhachHangId(User);
            if (khachHangId == null)
                return RedirectToAction("DangNhap", "TaiKhoan");

            // ✅ Lấy giỏ hàng chưa thanh toán
            var gioHangItems = await _context.GioHangItems
                .Where(i => i.LichSuMuaHangId == null) // Chưa có đơn hàng
                .ToListAsync();

            if (!gioHangItems.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng trống!";
                return RedirectToAction("Index", "GioHang");
            }

            // ✅ Tạo OrderId
            var orderId = $"ORD-{DateTime.Now:yyyyMMdd-HHmmss}-{new Random().Next(1000, 9999)}";

            // ✅ Bắt đầu Transaction để đảm bảo lưu dữ liệu đầy đủ
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // ✅ Tạo đơn hàng mới
                    var lichSuMuaHang = new LichSuMuaHang
                    {
                        KhachHangId = khachHangId,
                        NgayMua = DateTime.Now,
                        TongTien = gioHangItems.Sum(x => x.ThanhTien),
                        PhuongThucThanhToan = "Thanh toán khi nhận hàng",
                        DiaChiGiaoHang = "Địa chỉ mặc định",
                        Status = "Đang xử lý",
                        OrderId = orderId
                    };

                    _context.LichSuMuaHangs.Add(lichSuMuaHang);
                    await _context.SaveChangesAsync(); // ✅ Lưu đơn hàng vào DB

                    // ✅ Gán OrderId vào giỏ hàng
                    foreach (var item in gioHangItems)
                    {
                        item.LichSuMuaHangId = lichSuMuaHang.Id; // Liên kết với đơn hàng
                        item.NgayThanhToan = DateTime.Now;
                    }

                    _context.GioHangItems.UpdateRange(gioHangItems);
                    await _context.SaveChangesAsync(); // ✅ Lưu giỏ hàng vào DB

                    await transaction.CommitAsync(); // ✅ Xác nhận giao dịch
                    return RedirectToAction("CheckoutSuccess", "GioHang", new { id = lichSuMuaHang.Id });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(); // ❌ Hoàn tác nếu lỗi
                    TempData["ErrorMessage"] = "Lỗi thanh toán: " + ex.Message;
                    return RedirectToAction("Index", "GioHang");
                }
            }
        }
    }
}
