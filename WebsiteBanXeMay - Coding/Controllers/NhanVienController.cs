using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Controllers
{
    [Authorize] // Chỉ cho phép người đăng nhập
    public class NhanVienController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NhanVienController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 📌 1️⃣ Hiển thị danh sách nhân viên
        public async Task<IActionResult> Index()
        {
            var nhanViens = await _context.NhanViens
                .Include(nv => nv.User) // Load thông tin User để lấy Email
                .ToListAsync();

            return View(nhanViens);
        }

        // 📌 2️⃣ Xem chi tiết nhân viên
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var nhanVien = await _context.NhanViens
                .Include(nv => nv.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (nhanVien == null) return NotFound();

            return View(nhanVien);
        }

        // 📌 3️⃣ Thêm mới nhân viên (GET)
        public IActionResult Add()
        {
            ViewBag.Users = new SelectList(_context.Users
                .Where(u => !_context.NhanViens.Any(nv => nv.UserId == u.Id)), "Id", "Email");

            ViewBag.ChucVus = new SelectList(new[] { "Nhân viên", "Quản lý", "Admin" });
            return View();
        }

        // 📌 4️⃣ Thêm mới nhân viên (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhanVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Users = new SelectList(_context.Users
                .Where(u => !_context.NhanViens.Any(nv => nv.UserId == u.Id)), "Id", "Email");

            ViewBag.ChucVus = new SelectList(new[] { "Nhân viên", "Quản lý", "Admin" });
            return View(nhanVien);
        }

        // 📌 5️⃣ Chỉnh sửa nhân viên (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null) return NotFound();

            ViewBag.Users = new SelectList(_context.Users, "Id", "Email", nhanVien.UserId);
            ViewBag.ChucVus = new SelectList(new[] { "Nhân viên", "Quản lý", "Admin" }, nhanVien.ChucVu);
            return View(nhanVien);
        }

        // 📌 6️⃣ Chỉnh sửa nhân viên (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NhanVien nhanVien)
        {
            if (id != nhanVien.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhanVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.NhanViens.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Users = new SelectList(_context.Users, "Id", "Email", nhanVien.UserId);
            ViewBag.ChucVus = new SelectList(new[] { "Nhân viên", "Quản lý", "Admin" }, nhanVien.ChucVu);
            return View(nhanVien);
        }

        // 📌 7️⃣ Xóa nhân viên (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var nhanVien = await _context.NhanViens
                .Include(nv => nv.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (nhanVien == null) return NotFound();

            return View(nhanVien);
        }

        // 📌 8️⃣ Xóa nhân viên (POST)
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien != null)
            {
                _context.NhanViens.Remove(nhanVien);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
