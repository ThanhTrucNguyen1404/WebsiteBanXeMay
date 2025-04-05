using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;
using WebsiteBanXeMay___Coding.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WebsiteBanXeMay___Coding.Helpers; // Thêm namespace cho IOrderRepository

namespace WebsiteBanXeMay___Coding.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderRepository _orderRepository;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IOrderRepository orderRepository)
        {
            _context = context;
            _userManager = userManager;
            _orderRepository = orderRepository;
        }

        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index", "ShoppingCart");
            }

            var order = new Order
            {
                TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity)
            };

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(Order order)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng trống, không thể đặt hàng!";
                return RedirectToAction("Index", "ShoppingCart");
            }

            // Gán thông tin đơn hàng
            order.UserId = user.Id;
            order.OrderDate = DateTime.Now;
            order.Status = "Pending";
            order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
            order.OrderDetails = cart.Items.Select(i => new OrderDetail
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList();

            // Thêm đơn hàng vào database
            await _orderRepository.AddAsync(order);

            // Xóa giỏ hàng sau khi đặt hàng thành công
            HttpContext.Session.Remove("Cart");

            if (order.PaymentMethod == "COD")
            {
                return RedirectToAction("OrderSuccess", new { id = order.OrderId });
            }
            else if (order.PaymentMethod == "BankTransfer")
            {
                return RedirectToAction("BankTransferInstructions", new { id = order.OrderId });
            }

            TempData["ErrorMessage"] = "Phương thức thanh toán không hợp lệ!";
            return RedirectToAction("Checkout");
        }

        public async Task<IActionResult> OrderSuccess(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Mã đơn hàng không hợp lệ!";
                return RedirectToAction("Index", "Home");
            }

            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng!";
                return RedirectToAction("Index", "Home");
            }

            return View(order);
        }

        public async Task<IActionResult> BankTransferInstructions(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Mã đơn hàng không hợp lệ!";
                return RedirectToAction("Index", "Home");
            }

            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng!";
                return RedirectToAction("Index", "Home");
            }

            return View(order);
        }

        [Authorize]
        public async Task<IActionResult> History()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var orders = await _orderRepository.GetOrdersByUserAsync(userId);
            return View(orders);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Mã đơn hàng không hợp lệ.");
            }

            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound("Không tìm thấy đơn hàng.");
            }

            return View(order);
        }
    }
}
