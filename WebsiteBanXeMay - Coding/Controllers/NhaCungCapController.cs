using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Controllers
{
    public class NhaCungCapController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NhaCungCapController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🏠 1️⃣ Index - Danh sách nhà cung cấp
        public async Task<IActionResult> Index()
        {
            var list = await _context.NhaCungCaps.ToListAsync();
            return View(list);
        }

        // 🔍 2️⃣ Details - Xem chi tiết nhà cung cấp
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var nhaCungCap = await _context.NhaCungCaps.FirstOrDefaultAsync(nc => nc.Id == id);
            if (nhaCungCap == null) return NotFound();

            return View(nhaCungCap);
        }

        // ➕ 3️⃣ Add - Hiển thị form thêm mới
        public IActionResult Add()
        {
            return View();
        }

        // 🚀 Xử lý thêm nhà cung cấp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(NhaCungCap nhaCungCap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhaCungCap);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thêm nhà cung cấp thành công!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Có lỗi xảy ra khi thêm nhà cung cấp!";
            return View(nhaCungCap);
        }

        // ✏️ 4️⃣ Edit - Hiển thị form chỉnh sửa
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var nhaCungCap = await _context.NhaCungCaps.FindAsync(id);
            if (nhaCungCap == null) return NotFound();

            return View(nhaCungCap);
        }

        // 🚀 Xử lý cập nhật nhà cung cấp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NhaCungCap nhaCungCap)
        {
            if (id != nhaCungCap.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(nhaCungCap);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật thành công!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Có lỗi khi cập nhật!";
            return View(nhaCungCap);
        }

        // 🗑 5️⃣ Delete - Hiển thị xác nhận xóa
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var nhaCungCap = await _context.NhaCungCaps.FirstOrDefaultAsync(nc => nc.Id == id);
            if (nhaCungCap == null) return NotFound();

            return View(nhaCungCap);
        }

        // 🚀 Xử lý xóa nhà cung cấp
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhaCungCap = await _context.NhaCungCaps.FindAsync(id);
            if (nhaCungCap != null)
            {
                _context.NhaCungCaps.Remove(nhaCungCap);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xóa thành công!";
            }
            else
            {
                TempData["Error"] = "Không tìm thấy nhà cung cấp!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
