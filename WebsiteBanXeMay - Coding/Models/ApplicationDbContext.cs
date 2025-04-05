using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebsiteBanXeMay___Coding.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ChiTietDonDatHang> ChiTietDonDatHangs { get; set; }
        public DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
        public DbSet<DonDatHang> DonDatHangs { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<LoaiSanPham> LoaiSanPhams { get; set; }
        public DbSet<LoaiThanhVien> LoaiThanhViens { get; set; }
        public DbSet<LoaiThanhVien_Quyen> LoaiThanhVien_Quyens { get; set; }
        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<NhaSanXuat> NhaSanXuats { get; set; }
        public DbSet<PhieuNhap> PhieuNhaps { get; set; }
        public DbSet<Quyen> Quyens { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<ThanhVien> ThanhViens { get; set; }
        public DbSet<Xe> Xes { get; set; }
        public DbSet<ThanhToan> ThanhToans { get; set; }
        public DbSet<HinhAnh> HinhAnhs { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<GioHangItem> GioHangItems { get; set; }
        public DbSet<LichSuMuaHang> LichSuMuaHangs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔹 Cấu hình khóa chính cho bảng trung gian LoaiThanhVien_Quyen
            modelBuilder.Entity<LoaiThanhVien_Quyen>()
                .HasKey(lq => new { lq.LoaiThanhVienId, lq.QuyenId });

            // 🔹 Quan hệ LoaiThanhVien ↔ LoaiThanhVien_Quyen
            modelBuilder.Entity<LoaiThanhVien_Quyen>()
                .HasOne(lq => lq.LoaiThanhVien)
                .WithMany(ltv => ltv.LoaiThanhVien_Quyens)
                .HasForeignKey(lq => lq.LoaiThanhVienId)
                .OnDelete(DeleteBehavior.Cascade); // Nếu xóa LoaiThanhVien → Xóa luôn trong bảng trung gian

            // 🔹 Quan hệ Quyen ↔ LoaiThanhVien_Quyen
            modelBuilder.Entity<LoaiThanhVien_Quyen>()
                .HasOne(lq => lq.Quyen)
                .WithMany(q => q.LoaiThanhVien_Quyens)
                .HasForeignKey(lq => lq.QuyenId)
                .OnDelete(DeleteBehavior.Cascade); // Nếu xóa Quyen → Xóa luôn trong bảng trung gian

            // 🔹 Đặt tên bảng chuẩn (nếu cần)
            modelBuilder.Entity<LoaiThanhVien_Quyen>().ToTable("LoaiThanhVien_Quyen");

            modelBuilder.Entity<GioHangItem>()
            .HasOne(g => g.SanPham)
            .WithMany(s => s.CartItems)
            .HasForeignKey(g => g.SanPhamId)
            .OnDelete(DeleteBehavior.Cascade); // Xóa sản phẩm sẽ xóa cả giỏ hàng liên quan

            modelBuilder.Entity<PhieuNhap>()
        .HasOne(p => p.NhanVien)
        .WithMany()
        .HasForeignKey(p => p.NhanVienId)
        .OnDelete(DeleteBehavior.Restrict); // ✅ Tránh lỗi cascade delete

            modelBuilder.Entity<GioHangItem>()
        .Property(g => g.OrderId)
        .HasColumnType("nvarchar(50)"); // 🔥 Đảm bảo OrderId là string trong DB
        }
    }
}
