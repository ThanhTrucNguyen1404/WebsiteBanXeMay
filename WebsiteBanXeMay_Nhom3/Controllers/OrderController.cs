﻿using WebsiteBanXeMay_Nhom3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebsiteBanXeMay_Nhom3.Extensions;
using WebsiteBanXeMay_Nhom3.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;  // Thêm namespace cho IOrderRepository

namespace WebsiteBanXeMay_Nhom3.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderRepository _orderRepository; // Inject IOrderRepository

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IOrderRepository orderRepository)
        {
            _context = context;
            _userManager = userManager;
            _orderRepository = orderRepository;  // Thêm vào constructor
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

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Xóa giỏ hàng sau khi đặt hàng thành công
            HttpContext.Session.Remove("Cart");

            if (order.PaymentMethod == "COD")
            {
                return RedirectToAction("OrderSuccess", new { id = order.Id });
            }
            else if (order.PaymentMethod == "BankTransfer")
            {
                return RedirectToAction("BankTransferInstructions", new { id = order.Id });
            }

            TempData["ErrorMessage"] = "Phương thức thanh toán không hợp lệ!";
            return RedirectToAction("Checkout");
        }

        public async Task<IActionResult> OrderSuccess(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng!";
                return RedirectToAction("Index", "Home");
            }

            return View(order);
        }

        public async Task<IActionResult> BankTransferInstructions(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng!";
                return RedirectToAction("Index", "Home");
            }

            return View(order);
        }

        [Authorize] // Chỉ cho phép người dùng đã đăng nhập
        public async Task<IActionResult> History()
        {
            var userId = _userManager.GetUserId(User);
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .OrderByDescending(o => o.OrderDate)
                .AsNoTracking()  // ⚡ Quan trọng: Lấy dữ liệu mới nhất, không dùng cache
                .ToListAsync();

            return View(orders);
        }


        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product) // Đảm bảo lấy cả thông tin sản phẩm
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(Order order)
        {
            if (!ModelState.IsValid)
            {
                return View(order);
            }

            var existingOrder = await _context.Orders
                .Include(o => o.OrderDetails)  // ⚡ Đảm bảo OrderDetails không bị mất
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            if (existingOrder == null)
            {
                TempData["ErrorMessage"] = "❌ Không tìm thấy đơn hàng!";
                return NotFound();
            }

            // ⚡ Cập nhật dữ liệu đơn hàng
            existingOrder.ShippingAddress = order.ShippingAddress;
            existingOrder.Notes = order.Notes;
            existingOrder.Status = order.Status;
            existingOrder.TotalPrice = order.TotalPrice;  // Đảm bảo không bị reset
            existingOrder.PaymentMethod = order.PaymentMethod;

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "✅ Cập nhật đơn hàng thành công!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi khi lưu: {ex.Message}");
                TempData["ErrorMessage"] = "❌ Lỗi hệ thống khi lưu đơn hàng!";
            }

            return RedirectToAction("History");
        }



        [Authorize]
        public async Task<IActionResult> RemoveItem(int orderId, int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null || order.Status != "Chờ xác nhận")
            {
                return NotFound();
            }

            var itemToRemove = order.OrderDetails.FirstOrDefault(d => d.ProductId == productId);
            if (itemToRemove != null)
            {
                order.OrderDetails.Remove(itemToRemove);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Edit", new { id = orderId });
        }
    }
}