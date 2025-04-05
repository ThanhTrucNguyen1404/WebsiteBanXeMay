using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;
using WebsiteBanXeMay___Coding.Repositories;
using WebsiteBanXeMay___Coding.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ 1. Cấu hình DbContext sử dụng SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ 2. Đăng ký Identity (ApplicationUser & Role)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Để dễ dàng đăng nhập mà không yêu cầu xác nhận
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

// ✅ 3. Đăng ký Repository Pattern (Tránh lỗi phụ thuộc)
builder.Services.AddScoped<IChiTietPhieuNhapRepository, ChiTietPhieuNhapRepository>();
builder.Services.AddScoped<IDonDatHangRepository, DonDatHangRepository>();
builder.Services.AddScoped<IHinhAnhRepository, HinhAnhRepository>();
builder.Services.AddScoped<IKhachHangRepository, KhachHangRepository>();
builder.Services.AddScoped<ILoaiSanPhamRepository, LoaiSanPhamRepository>();
builder.Services.AddScoped<ILoaiThanhVienRepository, LoaiThanhVienRepository>();
builder.Services.AddScoped<ILoaiThanhVienQuyenRepository, LoaiThanhVienQuyenRepository>();
builder.Services.AddScoped<INhaCungCapRepository, NhaCungCapRepository>();
builder.Services.AddScoped<INhaSanXuatRepository, NhaSanXuatRepository>();
builder.Services.AddScoped<IPhieuNhapRepository, PhieuNhapRepository>();
builder.Services.AddScoped<IQuyenRepository, QuyenRepository>();
builder.Services.AddScoped<ISanPhamRepository, SanPhamRepository>();
builder.Services.AddScoped<IThanhToanRepository, ThanhToanRepository>();
builder.Services.AddScoped<IThanhVienRepository, ThanhVienRepository>();
builder.Services.AddScoped<IXeRepository, XeRepository>();
builder.Services.AddScoped<INhanVienRepository, NhanVienRepository>();
builder.Services.AddScoped<ILichSuMuaHangRepository, LichSuMuaHangRepository>();
builder.Services.AddScoped<IGioHangRepository, GioHangRepository>(); // ✅ Thêm dòng này
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();



// ✅ 4. Đăng ký Distributed Memory Cache (bắt buộc cho Session)
builder.Services.AddDistributedMemoryCache();

// ✅ 5. Cấu hình Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // ⏳ Timeout session sau 30 phút
    options.Cookie.HttpOnly = true; // 🔒 Tăng bảo mật
    options.Cookie.IsEssential = true; // ✅ Đảm bảo cookie session được sử dụng
});

// ✅ 6. Đăng ký MVC & Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build(); // 🔥 Gọi `Build()` sau khi đã đăng ký dịch vụ

// ✅ 7. Khởi tạo dữ liệu vai trò và tài khoản admin mặc định
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await CreateRolesAndAdmin(services);
}

// ✅ 8. Cấu hình Middleware (Xử lý lỗi, bảo mật)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Kích hoạt bảo mật cho môi trường không phải phát triển
}

app.UseHttpsRedirection(); // Chuyển hướng HTTP sang HTTPS
app.UseStaticFiles(); // Đảm bảo rằng các tệp tĩnh (CSS, JS, hình ảnh) được phục vụ

// ✅ 9. Middleware cho Session (Đặt TRƯỚC `UseRouting()`)
app.UseSession();

app.UseRouting();

app.UseAuthentication(); // ✅ Xác thực trước khi kiểm tra quyền hạn
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // 🔥 Cần thiết nếu bạn dùng Identity UI

// ✅ 10. Chạy ứng dụng
app.Run();

// ✅ Hàm tạo vai trò và tài khoản admin mặc định
async Task CreateRolesAndAdmin(IServiceProvider services)
{
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    // Tạo các vai trò nếu chưa có
    string[] roleNames = new string[] { "Admin", "User", "NhanVien" }; // Thêm vai trò "NhanVien"
    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Tạo tài khoản admin nếu chưa có
    var adminUser = await userManager.FindByEmailAsync("admin123@gmail.com");
    if (adminUser == null)
    {
        var user = new ApplicationUser
        {
            UserName = "admin123@gmail.com",
            Email = "admin123@gmail.com",
            FullName = "Admin User"
        };

        var result = await userManager.CreateAsync(user, "AnhKhoa@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }

    // Tạo tài khoản nhân viên nếu chưa có (ví dụ với email nhân viên)
    var employeeUser = await userManager.FindByEmailAsync("Nhanvien123@gmail.com");
    if (employeeUser == null)
    {
        var user = new ApplicationUser
        {
            UserName = "Nhanvien123@gmail.com",
            Email = "Nhanvien123@gmail.com",
            FullName = "Nhân viên"
        };

        var result = await userManager.CreateAsync(user, "NhanVien@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "NhanVien"); // Gán vai trò Nhân viên
        }
    }
}
