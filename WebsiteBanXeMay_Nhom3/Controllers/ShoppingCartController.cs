using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebsiteBanXeMay_Nhom3.Extensions;
using WebsiteBanXeMay_Nhom3.Models;
using WebsiteBanXeMay_Nhom3.Repositories;

namespace we.Controllers
{
    [Authorize(Roles = SD.Role_Customer)] // Yêu cầu đăng nhập và phải là khách hàng
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShoppingCartController(IProductRepository productRepository, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
        }

        // Hiển thị trang thanh toán
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            var cartItems = cart.Items; // Lấy danh sách sản phẩm trong giỏ hàng

            var order = new Order
            {
                TotalPrice = cartItems.Sum(item => item.Price * item.Quantity)
            };

            ViewData["CartItems"] = cartItems; // Truyền danh sách sản phẩm vào View
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");

            if (cart == null || !cart.Items.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để đặt hàng.";
                return RedirectToAction("Login", "Account");
            }

            // Tạo đơn hàng
            order.UserId = user.Id;
            order.OrderDate = DateTime.UtcNow;
            order.TotalPrice = cart.Items.Sum(item => item.Price * item.Quantity);
            order.Status = "Chờ xác nhận";
            order.OrderDetails = cart.Items.Select(i => new OrderDetail
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList();

            _context.Orders.Add(order);

            // 🔥 Cập nhật số lượng sản phẩm trong kho
            foreach (var item in cart.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null && product.Quantity >= item.Quantity)
                {
                    product.Quantity -= item.Quantity; // Trừ số lượng đã mua
                }
                else
                {
                    TempData["ErrorMessage"] = $"Sản phẩm {item.Name} không đủ hàng!";
                    return RedirectToAction("Index");
                }
            }

            await _context.SaveChangesAsync(); // ✅ Lưu thay đổi vào database

            HttpContext.Session.Remove("Cart"); // Xóa giỏ hàng sau khi đặt hàng

            return RedirectToAction("OrderCompleted", new { orderId = order.Id });
        }


        public async Task<IActionResult> OrderCompleted(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product) // Lấy thông tin sản phẩm từ OrderDetails
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["CartItems"] = order.OrderDetails.Select(od => new CartItem
            {
                ProductId = od.ProductId,
                Name = od.Product.Name,
                ImageUrl = od.Product.ImageUrl,
                Price = od.Price,
                Quantity = od.Quantity
            }).ToList();

            return View(order);
        }


        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại!";
                return RedirectToAction("Index", "Home");
            }

            // 🖼️ Chỉ lưu tên file, bỏ đi "wwwroot/Images/"
            string imageUrl = product.ImageUrl.Replace("wwwroot/Images/", "").Trim();

            Console.WriteLine($"🖼️ ImageUrl của {product.Name}: {imageUrl}"); // Debug

            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            cart.AddItem(new CartItem
            {
                ProductId = productId,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = imageUrl,  // ✅ Đã chuẩn hóa đường dẫn
                Quantity = quantity
            });

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return RedirectToAction("Index");
        }


        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            return View(cart);
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || cart.Items.All(i => i.ProductId != productId))
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng." });
            }

            cart.RemoveItem(productId);
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return Json(new { success = true });
        }

        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng!" });
            }

            // Kiểm tra số lượng tồn kho từ database
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return Json(new { success = false, message = "Sản phẩm không hợp lệ!" });
            }

            if (product.Quantity == 0)
            {
                return Json(new { success = false, message = "Sản phẩm đã hết hàng!" });
            }

            if (quantity > product.Quantity)
            {
                return Json(new
                {
                    success = false,
                    message = $"Số lượng vượt quá tồn kho! Chỉ còn {product.Quantity} sản phẩm."
                });
            }

            // Cập nhật số lượng trong giỏ hàng
            item.Quantity = quantity;
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return Json(new { success = true });
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null || order.Status != "Chờ xác nhận")
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
