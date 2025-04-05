using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;
using WebsiteBanXeMay___Coding.Repositories;

namespace WebsiteBanXeMay___Coding.Controllers
{
    public class QuyenController : Controller
    {
        private readonly IQuyenRepository _repository;

        public QuyenController(IQuyenRepository repository)
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

            var quyen = await _repository.GetByIdAsync(id.Value);
            if (quyen == null) return NotFound();

            return View(quyen);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenQuyen,MoTa")] Quyen quyen)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(quyen);
                return RedirectToAction(nameof(Index));
            }
            return View(quyen);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var quyen = await _repository.GetByIdAsync(id.Value);
            if (quyen == null) return NotFound();

            return View(quyen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenQuyen,MoTa")] Quyen quyen)
        {
            if (id != quyen.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(quyen);
                return RedirectToAction(nameof(Index));
            }
            return View(quyen);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var quyen = await _repository.GetByIdAsync(id.Value);
            if (quyen == null) return NotFound();

            return View(quyen);
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
