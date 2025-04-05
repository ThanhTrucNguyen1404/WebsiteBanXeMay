using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Controllers
{
    public class XeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public XeController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // 📌 1️⃣ Hiển thị danh sách xe
        public async Task<IActionResult> Index()
        {
            var xes = await _context.Xes.Include(x => x.LoaiSanPham).Include(x => x.NhaSanXuat).ToListAsync();
            return View(xes);
        }

        // 📌 2️⃣ Hiển thị chi tiết xe
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var xe = await _context.Xes
                .Include(x => x.LoaiSanPham)
                .Include(x => x.NhaSanXuat)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (xe == null) return NotFound();

            return View(xe);
        }

        // 📌 3️⃣ Thêm mới xe (GET)
        public IActionResult Create()
        {
            return View();
        }

        // 📌 4️⃣ Thêm mới xe (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Xe xe)
        {
            if (ModelState.IsValid)
            {
                // 🔥 Xử lý hình ảnh
                if (xe.ImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(xe.ImageFile.FileName);
                    string filePath = Path.Combine(wwwRootPath + "/Images", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await xe.ImageFile.CopyToAsync(fileStream);
                    }
                    xe.HinhAnh = fileName;
                }

                _context.Add(xe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(xe);
        }

        // 📌 5️⃣ Chỉnh sửa xe (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var xe = await _context.Xes.FindAsync(id);
            if (xe == null) return NotFound();

            return View(xe);
        }

        // 📌 6️⃣ Chỉnh sửa xe (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Xe xe)
        {
            if (id != xe.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // 🔥 Xử lý cập nhật hình ảnh
                    if (xe.ImageFile != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(xe.ImageFile.FileName);
                        string filePath = Path.Combine(wwwRootPath + "/Images", fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await xe.ImageFile.CopyToAsync(fileStream);
                        }

                        // Xóa ảnh cũ nếu có
                        if (!string.IsNullOrEmpty(xe.HinhAnh))
                        {
                            string oldImagePath = Path.Combine(wwwRootPath + "/Images", xe.HinhAnh);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        xe.HinhAnh = fileName;
                    }

                    _context.Update(xe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Xes.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(xe);
        }

        // 📌 7️⃣ Xóa xe (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var xe = await _context.Xes.FirstOrDefaultAsync(m => m.Id == id);
            if (xe == null) return NotFound();

            return View(xe);
        }

        // 📌 8️⃣ Xóa xe (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var xe = await _context.Xes.FindAsync(id);
            if (xe != null)
            {
                // 🔥 Xóa hình ảnh nếu có
                if (!string.IsNullOrEmpty(xe.HinhAnh))
                {
                    string filePath = Path.Combine(_hostEnvironment.WebRootPath + "/Images", xe.HinhAnh);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Xes.Remove(xe);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
