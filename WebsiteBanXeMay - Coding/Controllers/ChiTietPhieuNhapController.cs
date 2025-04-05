using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteBanXeMay___Coding.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebsiteBanXeMay___Coding.Controllers
{
    [Authorize] // Yêu cầu đăng nhập
    public class ChiTietPhieuNhapController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChiTietPhieuNhapController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 📌 1️⃣ Hiển thị danh sách chi tiết phiếu nhập
        public async Task<IActionResult> Index()
        {
            var chiTietPhieuNhaps = await _context.ChiTietPhieuNhaps
                .Include(c => c.PhieuNhap) // Load thông tin phiếu nhập
                .Include(c => c.Xe)        // Load thông tin xe nhập
                .ToListAsync();

            return View(chiTietPhieuNhaps);
        }

        // 📌 2️⃣ Hiển thị chi tiết một chi tiết phiếu nhập
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var chiTietPhieuNhap = await _context.ChiTietPhieuNhaps
                .Include(c => c.PhieuNhap)
                .Include(c => c.Xe)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (chiTietPhieuNhap == null) return NotFound();

            return View(chiTietPhieuNhap);
        }

        // 📌 3️⃣ Thêm mới chi tiết phiếu nhập (GET)
        public IActionResult Create()
        {
            ViewBag.PhieuNhaps = _context.PhieuNhaps.Select(p => new { p.Id, p.NgayNhap }).ToList();
            ViewBag.Xes = _context.Xes.Select(x => new { x.Id, x.TenXe }).ToList();
            return View();
        }

        // 📌 4️⃣ Thêm mới chi tiết phiếu nhập (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChiTietPhieuNhap chiTietPhieuNhap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chiTietPhieuNhap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.PhieuNhaps = _context.PhieuNhaps.Select(p => new { p.Id, p.NgayNhap }).ToList();
            ViewBag.Xes = _context.Xes.Select(x => new { x.Id, x.TenXe }).ToList();
            return View(chiTietPhieuNhap);
        }

        // 📌 5️⃣ Chỉnh sửa chi tiết phiếu nhập (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var chiTietPhieuNhap = await _context.ChiTietPhieuNhaps.FindAsync(id);
            if (chiTietPhieuNhap == null) return NotFound();

            ViewBag.PhieuNhaps = _context.PhieuNhaps.Select(p => new { p.Id, p.NgayNhap }).ToList();
            ViewBag.Xes = _context.Xes.Select(x => new { x.Id, x.TenXe }).ToList();
            return View(chiTietPhieuNhap);
        }

        // 📌 6️⃣ Chỉnh sửa chi tiết phiếu nhập (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ChiTietPhieuNhap chiTietPhieuNhap)
        {
            if (id != chiTietPhieuNhap.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTietPhieuNhap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ChiTietPhieuNhaps.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.PhieuNhaps = _context.PhieuNhaps.Select(p => new { p.Id, p.NgayNhap }).ToList();
            ViewBag.Xes = _context.Xes.Select(x => new { x.Id, x.TenXe }).ToList();
            return View(chiTietPhieuNhap);
        }

        // 📌 7️⃣ Xóa chi tiết phiếu nhập (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var chiTietPhieuNhap = await _context.ChiTietPhieuNhaps
                .Include(c => c.PhieuNhap)
                .Include(c => c.Xe)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (chiTietPhieuNhap == null) return NotFound();

            return View(chiTietPhieuNhap);
        }

        // 📌 8️⃣ Xóa chi tiết phiếu nhập (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chiTietPhieuNhap = await _context.ChiTietPhieuNhaps.FindAsync(id);
            if (chiTietPhieuNhap != null)
            {
                _context.ChiTietPhieuNhaps.Remove(chiTietPhieuNhap);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
