using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebsiteBanXeMay___Coding.Helpers;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Controllers
{
    public class GioHangController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager; // Add this line
        private const string CartSessionKey = "Cart";

        public GioHangController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) // Modify constructor
        {
            _context = context;
            _userManager = userManager; // Initialize _userManager
        }

        // Hiển thị giỏ hàng
        public IActionResult Index()
        {
            var cart = GetCartFromSession();
            return View(cart);
        }

        // Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = _context.SanPhams.Find(productId);
            if (product == null || product.SoLuongTonKho < quantity)
            {
                return BadRequest("Sản phẩm không khả dụng.");
            }

            var cart = GetCartFromSession();
            var existingItem = cart.FirstOrDefault(x => x.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new GioHangItem
                {
                    ProductId = product.Id,
                    Name = product.TenSanPham,
                    Price = product.GiaBan,
                    Quantity = quantity,
                    ImageUrl = product.HinhAnh ?? "/Images/no-image.png",
                    StockQuantity = product.SoLuongTonKho ?? 0 // Fix for CS0266 and CS8629
                });

            }

            SaveCartToSession(cart);
            return RedirectToAction("Index");
        }

        // Cập nhật số lượng sản phẩm trong giỏ hàng
        [HttpPost]
        public IActionResult UpdateCart(int productId, int quantity)
        {
            var product = _context.SanPhams.Find(productId);
            if (product == null || quantity > product.SoLuongTonKho)
            {
                TempData["Error"] = "Không đủ hàng.";
                return RedirectToAction("Index");
            }

            var cart = GetCartFromSession();
            var cartItem = cart.FirstOrDefault(x => x.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
            }

            SaveCartToSession(cart);
            return RedirectToAction("Index");
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCartFromSession();
            cart.RemoveAll(x => x.ProductId == productId);
            SaveCartToSession(cart);
            return RedirectToAction("Index");
        }

        // Xóa toàn bộ giỏ hàng
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CartSessionKey);
            return RedirectToAction("Index");
        }

        // Thanh toán
        [HttpPost]
        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = GetCartFromSession();
            if (!cart.Any()) return RedirectToAction("Index");

            return View(cart); // Hiển thị trang thanh toán trước
        }


        // Trang xác nhận thanh toán thành công
        public IActionResult CheckoutSuccess()
        {
            return View();
        }

        // Lấy giỏ hàng từ Session
        private List<GioHangItem> GetCartFromSession()
        {
            return HttpContext.Session.GetObjectFromJson<List<GioHangItem>>(CartSessionKey) ?? new List<GioHangItem>();
        }

        // Lưu giỏ hàng vào Session
        private void SaveCartToSession(List<GioHangItem> cart)
        {
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        }

        [HttpPost]
        public IActionResult ConfirmPayment(string FullName, string Address, string PaymentMethod)
        {
            var cart = GetCartFromSession();
            if (cart == null || !cart.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng trống!";
                return RedirectToAction("Index");
            }

            ViewBag.FullName = FullName;
            ViewBag.Address = Address;
            ViewBag.PaymentMethod = PaymentMethod;

            string orderId = Guid.NewGuid().ToString();
            ViewBag.OrderId = orderId;

            // Giảm số lượng tồn kho của sản phẩm
            foreach (var item in cart)
            {
                var product = _context.SanPhams.Find(item.ProductId);
                if (product != null && product.SoLuongTonKho >= item.Quantity)
                {
                    product.SoLuongTonKho -= item.Quantity;
                }
            }

            _context.SaveChanges();
            HttpContext.Session.Remove(CartSessionKey);

            return View("CheckoutSuccess", cart);
        }

        [Authorize]
        public IActionResult LichSuMuaHang()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return RedirectToAction("DangNhap", "TaiKhoan");

            var lichSu = _context.GioHangItems
                .Where(i => i.OrderId != null && i.OrderId != null) // Chỉ lấy đơn đã thanh toán
                .OrderByDescending(i => i.NgayThanhToan)
                .ToList();

            return View("LichSu", lichSu);
        }

        public IActionResult LichSu()
        {
            var lichSu = _context.GioHangItems
                .Where(i => i.OrderId != null)
                .OrderByDescending(i => i.NgayThanhToan)
                .ToList();

            return View("LichSu", lichSu);
        }
    }
}
