using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteBanXeMay___Coding.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebsiteBanXeMay___Coding.Controllers
{
    [Authorize] // Yêu cầu đăng nhập
    public class ChiTietDonDatHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChiTietDonDatHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 📌 1️⃣ Hiển thị danh sách chi tiết đơn hàng
        public async Task<IActionResult> Index()
        {
            var chiTietDonHangs = await _context.ChiTietDonDatHangs
                .Include(c => c.DonDatHang)
                .Include(c => c.Xe)
                .ToListAsync();
            return View(chiTietDonHangs);
        }

        // 📌 2️⃣ Hiển thị chi tiết một đơn hàng
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var chiTiet = await _context.ChiTietDonDatHangs
                .Include(c => c.DonDatHang)
                .Include(c => c.Xe)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (chiTiet == null) return NotFound();

            return View(chiTiet);
        }

        // 📌 3️⃣ Thêm mới chi tiết đơn hàng (GET)
        public IActionResult Create()
        {
            ViewBag.DonDatHangs = _context.DonDatHangs.Select(d => new { d.DonDatHangId }).ToList();
            ViewBag.Xes = _context.Xes.Select(x => new { x.Id, x.TenXe }).ToList();
            return View();
        }

        // 📌 4️⃣ Thêm mới chi tiết đơn hàng (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChiTietDonDatHang chiTietDonDatHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chiTietDonDatHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.DonDatHangs = _context.DonDatHangs.Select(d => new { d.DonDatHangId }).ToList();
            ViewBag.Xes = _context.Xes.Select(x => new { x.Id, x.TenXe }).ToList();
            return View(chiTietDonDatHang);
        }

        // 📌 5️⃣ Chỉnh sửa chi tiết đơn hàng (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var chiTiet = await _context.ChiTietDonDatHangs.FindAsync(id);
            if (chiTiet == null) return NotFound();

            ViewBag.DonDatHangs = _context.DonDatHangs.Select(d => new { d.DonDatHangId }).ToList();
            ViewBag.Xes = _context.Xes.Select(x => new { x.Id, x.TenXe }).ToList();
            return View(chiTiet);
        }

        // 📌 6️⃣ Chỉnh sửa chi tiết đơn hàng (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ChiTietDonDatHang chiTietDonDatHang)
        {
            if (id != chiTietDonDatHang.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTietDonDatHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ChiTietDonDatHangs.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.DonDatHangs = _context.DonDatHangs.Select(d => new { d.DonDatHangId }).ToList();
            ViewBag.Xes = _context.Xes.Select(x => new { x.Id, x.TenXe }).ToList();
            return View(chiTietDonDatHang);
        }

        // 📌 7️⃣ Xóa chi tiết đơn hàng (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var chiTiet = await _context.ChiTietDonDatHangs
                .Include(c => c.DonDatHang)
                .Include(c => c.Xe)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (chiTiet == null) return NotFound();

            return View(chiTiet);
        }

        // 📌 8️⃣ Xóa chi tiết đơn hàng (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chiTiet = await _context.ChiTietDonDatHangs.FindAsync(id);
            if (chiTiet != null)
            {
                _context.ChiTietDonDatHangs.Remove(chiTiet);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
