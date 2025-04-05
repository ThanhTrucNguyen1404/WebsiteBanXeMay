using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebsiteBanXeMay___Coding.Models;
using WebsiteBanXeMay___Coding.Repositories;

namespace WebsiteBanXeMay___Coding.Controllers
{
    public class ThanhToanController : Controller
    {
        private readonly IThanhToanRepository _repository;
        private readonly IDonDatHangRepository _donDatHangRepository;

        public ThanhToanController(IThanhToanRepository repository, IDonDatHangRepository donDatHangRepository)
        {
            _repository = repository;
            _donDatHangRepository = donDatHangRepository;
        }

        public async Task<IActionResult> Index()
        {
            var thanhToans = await _repository.GetAllAsync();
            return View(thanhToans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var thanhToan = await _repository.GetByIdAsync(id);
            if (thanhToan == null) return NotFound();
            return View(thanhToan);
        }

        public async Task<IActionResult> Create(int donDatHangId)
        {
            var donDatHang = await _donDatHangRepository.GetByIdAsync(donDatHangId);
            if (donDatHang == null) return NotFound();

            if (donDatHang.TrangThai == "Đã thanh toán")
            {
                TempData["ErrorMessage"] = "Đơn hàng đã được thanh toán trước đó!";
                return RedirectToAction(nameof(Index));
            }

            var thanhToan = new ThanhToan { DonDatHangId = donDatHangId };
            return View(thanhToan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DonDatHangId,PhuongThuc,SoTien,TrangThai")] ThanhToan thanhToan)
        {
            if (!thanhToan.DonDatHangId.HasValue)
            {
                ModelState.AddModelError("DonDatHangId", "Đơn đặt hàng không hợp lệ.");
            }
            else
            {
                var donDatHang = await _donDatHangRepository.GetByIdAsync(thanhToan.DonDatHangId.Value);

                if (donDatHang == null)
                {
                    ModelState.AddModelError("DonDatHangId", "Đơn đặt hàng không hợp lệ.");
                }
                else
                {
                    if (donDatHang.TrangThai == "Đã thanh toán")
                    {
                        ModelState.AddModelError("", "Đơn hàng này đã thanh toán trước đó!");
                    }

                    if (thanhToan.SoTien > donDatHang.TongTien)
                    {
                        ModelState.AddModelError("SoTien", "Số tiền thanh toán không thể lớn hơn tổng tiền đơn hàng.");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                await _repository.AddAsync(thanhToan);
                return RedirectToAction(nameof(Index));
            }

            return View(thanhToan);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var thanhToan = await _repository.GetByIdAsync(id);
            if (thanhToan == null) return NotFound();

            var donDatHang = await _donDatHangRepository.GetByIdAsync(thanhToan.DonDatHangId.Value);
            if (donDatHang.TrangThai == "Đã thanh toán")
            {
                TempData["ErrorMessage"] = "Không thể sửa thanh toán của đơn hàng đã thanh toán!";
                return RedirectToAction(nameof(Index));
            }

            return View(thanhToan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DonDatHangId,PhuongThuc,SoTien,TrangThai")] ThanhToan thanhToan)
        {
            if (id != thanhToan.Id) return NotFound();

            var donDatHang = await _donDatHangRepository.GetByIdAsync(thanhToan.DonDatHangId.Value);
            if (donDatHang == null)
            {
                ModelState.AddModelError("DonDatHangId", "Đơn đặt hàng không hợp lệ.");
            }
            else if (donDatHang.TrangThai == "Đã thanh toán")
            {
                ModelState.AddModelError("", "Không thể chỉnh sửa thanh toán của đơn hàng đã hoàn tất!");
            }
            else if (thanhToan.SoTien > donDatHang.TongTien)
            {
                ModelState.AddModelError("SoTien", "Số tiền thanh toán không thể lớn hơn tổng tiền đơn hàng.");
            }

            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(thanhToan);
                return RedirectToAction(nameof(Index));
            }
            return View(thanhToan);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var thanhToan = await _repository.GetByIdAsync(id);
            if (thanhToan == null) return NotFound();

            var donDatHang = await _donDatHangRepository.GetByIdAsync(thanhToan.DonDatHangId.Value);
            if (donDatHang.TrangThai == "Đã thanh toán")
            {
                TempData["ErrorMessage"] = "Không thể xóa thanh toán của đơn hàng đã thanh toán!";
                return RedirectToAction(nameof(Index));
            }

            return View(thanhToan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var thanhToan = await _repository.GetByIdAsync(id);
            if (thanhToan == null) return NotFound();

            var donDatHang = await _donDatHangRepository.GetByIdAsync(thanhToan.DonDatHangId.Value);
            if (donDatHang.TrangThai == "Đã thanh toán")
            {
                TempData["ErrorMessage"] = "Không thể xóa thanh toán của đơn hàng đã thanh toán!";
                return RedirectToAction(nameof(Index));
            }

            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
