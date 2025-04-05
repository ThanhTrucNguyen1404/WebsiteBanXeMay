using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;
using WebsiteBanXeMay___Coding.Repositories;

namespace WebsiteBanXeMay___Coding.Controllers
{
    public class LoaiThanhVien_QuyenController : Controller
    {
        private readonly ILoaiThanhVienQuyenRepository _repository;
        private readonly ApplicationDbContext _context;

        public LoaiThanhVien_QuyenController(ILoaiThanhVienQuyenRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _repository.GetAllAsync();
            return View(data);
        }

        public IActionResult Create()
        {
            ViewData["LoaiThanhVienId"] = new SelectList(_context.LoaiThanhViens, "Id", "TenLoai");
            ViewData["QuyenId"] = new SelectList(_context.Quyens, "Id", "TenQuyen");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LoaiThanhVienId,QuyenId")] LoaiThanhVien_Quyen loaiThanhVien_Quyen)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(loaiThanhVien_Quyen);
                return RedirectToAction(nameof(Index));
            }
            ViewData["LoaiThanhVienId"] = new SelectList(_context.LoaiThanhViens, "Id", "TenLoai", loaiThanhVien_Quyen.LoaiThanhVienId);
            ViewData["QuyenId"] = new SelectList(_context.Quyens, "Id", "TenQuyen", loaiThanhVien_Quyen.QuyenId);
            return View(loaiThanhVien_Quyen);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var loaiThanhVien_Quyen = await _repository.GetByIdAsync(id);
            if (loaiThanhVien_Quyen == null) return NotFound();
            return View(loaiThanhVien_Quyen);
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
