using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;
using WebsiteBanXeMay___Coding.Repositories;

namespace WebsiteBanXeMay___Coding.Controllers
{
    public class LoaiThanhVienController : Controller
    {
        private readonly ILoaiThanhVienRepository _repository;

        public LoaiThanhVienController(ILoaiThanhVienRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _repository.GetAllAsync();
            return View(data);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var loaiThanhVien = await _repository.GetByIdAsync(id.Value);
            if (loaiThanhVien == null) return NotFound();

            return View(loaiThanhVien);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenLoai,Mota")] LoaiThanhVien loaiThanhVien)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(loaiThanhVien);
                return RedirectToAction(nameof(Index));
            }
            return View(loaiThanhVien);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var loaiThanhVien = await _repository.GetByIdAsync(id.Value);
            if (loaiThanhVien == null) return NotFound();

            return View(loaiThanhVien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenLoai,Mota")] LoaiThanhVien loaiThanhVien)
        {
            if (id != loaiThanhVien.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(loaiThanhVien);
                return RedirectToAction(nameof(Index));
            }
            return View(loaiThanhVien);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var loaiThanhVien = await _repository.GetByIdAsync(id.Value);
            if (loaiThanhVien == null) return NotFound();

            return View(loaiThanhVien);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
