using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Controllers
{
    [Authorize]
    public class SanPhamController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SanPhamController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // 📌 Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var sanPhams = await _context.SanPhams
                                         .Where(sp => sp.SoLuongTonKho > 0)
                                         .Include(s => s.LoaiSanPham)
                                         .Include(s => s.NhaSanXuat)
                                         .Include(s => s.NhaCungCap)
                                         .AsNoTracking()
                                         .ToListAsync();
            return View(sanPhams);
        }

        // 📌 Hiển thị chi tiết sản phẩm
        public async Task<IActionResult> Details(int id)
        {
            var sanPham = await _context.SanPhams
                                        .Include(sp => sp.LoaiSanPham)
                                        .Include(sp => sp.NhaSanXuat)
                                        .Include(sp => sp.NhaCungCap)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(sp => sp.Id == id);
            if (sanPham == null) return NotFound();
            return View(sanPham);
        }

        // 📌 Thêm sản phẩm (GET)
        public IActionResult Add()
        {
            LoadDropdowns();
            return View();
        }

        // 📌 Xử lý thêm sản phẩm (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(SanPham sanPham, IFormFile? HinhAnhFile)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(sanPham);
            }

            if (HinhAnhFile != null && ValidateImage(HinhAnhFile))
            {
                sanPham.HinhAnh = UploadImage(HinhAnhFile);
            }

            _context.Add(sanPham);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // 📌 Chỉnh sửa sản phẩm (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null) return NotFound();

            LoadDropdowns();
            return View(sanPham);
        }

        // 📌 Xử lý chỉnh sửa sản phẩm (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SanPham sanPham, IFormFile? HinhAnhFile)
        {
            if (id != sanPham.Id) return NotFound();

            var existingSanPham = await _context.SanPhams.FindAsync(id);
            if (existingSanPham == null) return NotFound();

            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(sanPham);
            }

            if (HinhAnhFile != null && ValidateImage(HinhAnhFile))
            {
                if (!string.IsNullOrEmpty(existingSanPham.HinhAnh))
                {
                    DeleteImage(existingSanPham.HinhAnh);
                }
                sanPham.HinhAnh = UploadImage(HinhAnhFile);
            }
            else
            {
                sanPham.HinhAnh = existingSanPham.HinhAnh;
            }

            _context.Entry(existingSanPham).CurrentValues.SetValues(sanPham);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // 📌 Xóa sản phẩm (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var sanPham = await _context.SanPhams
                                        .Include(s => s.LoaiSanPham)
                                        .Include(s => s.NhaSanXuat)
                                        .Include(s => s.NhaCungCap)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(m => m.Id == id);

            if (sanPham == null) return NotFound();
            return View(sanPham);
        }

        // 📌 Xử lý xóa sản phẩm (POST)
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sanPham = await _context.SanPhams
                                        .Include(sp => sp.CartItems) // Check giỏ hàng
                                        .FirstOrDefaultAsync(sp => sp.Id == id);

            if (sanPham == null) return NotFound();
            if (sanPham.CartItems.Any())
            {
                TempData["Error"] = "Không thể xóa vì sản phẩm đang trong giỏ hàng!";
                return RedirectToAction(nameof(Delete), new { id });
            }

            if (!string.IsNullOrEmpty(sanPham.HinhAnh))
            {
                DeleteImage(sanPham.HinhAnh);
            }

            _context.SanPhams.Remove(sanPham);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // 📌 Xử lý hình ảnh
        private string UploadImage(IFormFile file)
        {
            string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return "/Images/" + uniqueFileName;
        }

        private void DeleteImage(string imagePath)
        {
            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

        private bool ValidateImage(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            return allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower());
        }

        // 📌 Load danh sách dropdown
        private void LoadDropdowns()
        {
            ViewBag.LoaiSanPhamId = new SelectList(_context.LoaiSanPhams?.ToList() ?? new List<LoaiSanPham>(), "Id", "TenLoai");
            ViewBag.NhaSanXuatId = new SelectList(_context.NhaSanXuats?.ToList() ?? new List<NhaSanXuat>(), "Id", "TenNhaSanXuat");
            ViewBag.NhaCungCapId = new SelectList(_context.NhaCungCaps?.ToList() ?? new List<NhaCungCap>(), "Id", "TenNhaCungCap");
        }

        // 📌 Thêm sản phẩm vào giỏ hàng
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            return RedirectToAction("AddToCart", "Cart", new { id, quantity });
        }

        public IActionResult Checkout()
        {
            var cartItems = _context.GioHangItems
                                    .Include(g => g.SanPham)
                                    .ToList();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Giỏ hàng trống!";
                return RedirectToAction("Index", "GioHang");
            }

            return View(cartItems); // Không chỉ định tên file, MVC sẽ tìm Views/SanPham/Checkout.cshtml
        }
    }
}
