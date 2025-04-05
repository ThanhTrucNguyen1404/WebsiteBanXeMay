using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteBanXeMay___Coding.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace WebsiteBanXeMay___Coding.Controllers
{
    [Authorize(Roles = "Admin")] // Require Admin role for access
    public class KhachHangController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public KhachHangController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 📌 1️⃣ Display list of customers
        public async Task<IActionResult> Index()
        {
            var khachHangs = await _context.KhachHangs.ToListAsync();
            return View(khachHangs); // Display the list of customers
        }

        // 📌 2️⃣ View customer details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (khachHang == null) return NotFound();

            return View(khachHang);
        }

        // 📌 3️⃣ Add new customer (GET)
        [HttpGet]
        public IActionResult Add()
        {
            ViewData["Title"] = "Thêm Mới Khách Hàng";
            return View(new KhachHang()); // Initialize a new KhachHang object if required
        }

        // 📌 4️⃣ Add new customer (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(KhachHang khachHang)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(khachHang);
            }

            // Set the UserId to the currently logged-in user's Id
            khachHang.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Or use User.Identity.Name depending on your setup

            _context.Add(khachHang);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // 📌 5️⃣ Edit customer (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var khachHang = await _context.KhachHangs
                .AsNoTracking() // Prevents tracking the entity
                .FirstOrDefaultAsync(m => m.Id == id);

            if (khachHang == null) return NotFound();

            return View(khachHang); // Return the customer data to the edit view
        }

        // 📌 6️⃣ Edit customer (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KhachHang khachHang)
        {
            if (id != khachHang.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the existing customer from the database without tracking it
                    var existingCustomer = await _context.KhachHangs
                        .AsNoTracking() // Ensure the customer is not being tracked
                        .FirstOrDefaultAsync(x => x.Id == id);

                    if (existingCustomer == null) return NotFound();

                    // Preserve the UserId and other necessary fields
                    khachHang.UserId = existingCustomer.UserId;

                    // Explicitly mark the entity as modified
                    _context.Entry(khachHang).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.KhachHangs.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(khachHang); // Return to the Edit view if validation fails
        }

        // 📌 7️⃣ Delete customer (GET)
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (khachHang == null) return NotFound();

            return View(khachHang);
        }

        // 📌 8️⃣ Delete customer (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang != null)
            {
                _context.KhachHangs.Remove(khachHang);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index)); // Redirect to list of customers after deleting
        }
    }
}
