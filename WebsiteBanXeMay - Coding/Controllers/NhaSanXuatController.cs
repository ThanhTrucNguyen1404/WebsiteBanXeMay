using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Controllers
{
    public class NhaSanXuatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NhaSanXuatController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NhaSanXuats
        public async Task<IActionResult> Index()
        {
            return View(await _context.NhaSanXuats.ToListAsync());
        }

        // GET: NhaSanXuats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var nhaSanXuat = await _context.NhaSanXuats
                .FirstOrDefaultAsync(m => m.Id == id);

            if (nhaSanXuat == null) return NotFound();

            return View(nhaSanXuat);
        }

        // GET: NhaSanXuats/Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: NhaSanXuats/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Id,TenNhaSanXuat,DiaChi,SoDienThoai,Email,MoTa")] NhaSanXuat nhaSanXuat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhaSanXuat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nhaSanXuat);
        }

        // GET: NhaSanXuats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var nhaSanXuat = await _context.NhaSanXuats.FindAsync(id);
            if (nhaSanXuat == null) return NotFound();

            return View(nhaSanXuat);
        }

        // POST: NhaSanXuats/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenNhaSanXuat,DiaChi,SoDienThoai,Email,MoTa")] NhaSanXuat nhaSanXuat)
        {
            if (id != nhaSanXuat.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhaSanXuat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhaSanXuatExists(nhaSanXuat.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nhaSanXuat);
        }

        // GET: NhaSanXuats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var nhaSanXuat = await _context.NhaSanXuats
                .FirstOrDefaultAsync(m => m.Id == id);

            if (nhaSanXuat == null) return NotFound();

            return View(nhaSanXuat);
        }

        // POST: NhaSanXuats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhaSanXuat = await _context.NhaSanXuats.FindAsync(id);
            if (nhaSanXuat != null)
            {
                _context.NhaSanXuats.Remove(nhaSanXuat);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool NhaSanXuatExists(int id)
        {
            return _context.NhaSanXuats.Any(e => e.Id == id);
        }
    }
}
