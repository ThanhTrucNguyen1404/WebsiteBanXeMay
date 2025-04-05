using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Controllers
{
    public class LoaiSanPhamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoaiSanPhamController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LoaiSanPhams
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoaiSanPhams.ToListAsync());
        }

        // GET: LoaiSanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var loaiSanPham = await _context.LoaiSanPhams
                .FirstOrDefaultAsync(m => m.Id == id);

            if (loaiSanPham == null) return NotFound();

            return View(loaiSanPham);
        }

        // GET: LoaiSanPhams/Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: LoaiSanPhams/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Id,TenLoai,MoTa")] LoaiSanPham loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loaiSanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loaiSanPham);
        }

        // GET: LoaiSanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var loaiSanPham = await _context.LoaiSanPhams.FindAsync(id);
            if (loaiSanPham == null) return NotFound();

            return View(loaiSanPham);
        }

        // POST: LoaiSanPhams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenLoai,MoTa")] LoaiSanPham loaiSanPham)
        {
            if (id != loaiSanPham.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiSanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiSanPhamExists(loaiSanPham.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(loaiSanPham);
        }

        // GET: LoaiSanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var loaiSanPham = await _context.LoaiSanPhams
                .FirstOrDefaultAsync(m => m.Id == id);

            if (loaiSanPham == null) return NotFound();

            return View(loaiSanPham);
        }

        // POST: LoaiSanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loaiSanPham = await _context.LoaiSanPhams.FindAsync(id);
            if (loaiSanPham != null)
            {
                _context.LoaiSanPhams.Remove(loaiSanPham);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool LoaiSanPhamExists(int id)
        {
            return _context.LoaiSanPhams.Any(e => e.Id == id);
        }
    }
}
