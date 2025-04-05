using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteBanXeMay___Coding.Models
{
    public class ChiTietPhieuNhap
    {
        [Key]
        public int Id { get; set; }

        // 🔹 Khóa ngoại Phiếu Nhập
        [Required]
        public int PhieuNhapId { get; set; }

        [ForeignKey("PhieuNhapId")]
        public virtual PhieuNhap? PhieuNhap { get; set; }

        // 🔹 Khóa ngoại Xe
        [Required]
        public int XeId { get; set; }

        [ForeignKey("XeId")]
        public virtual Xe? Xe { get; set; }

        // 🔹 Số Lượng
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int SoLuong { get; set; }

        // 🔹 Giá Nhập (decimal 18,2)
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá nhập phải lớn hơn hoặc bằng 0.")]
        public decimal GiaNhap { get; set; }

        // 🔹 Thành Tiền (Tính toán tự động)
        [NotMapped]
        public decimal ThanhTien => SoLuong * GiaNhap;

        // 🔹 Danh sách hình ảnh của sản phẩm nhập
        public virtual ICollection<HinhAnh> HinhAnhs { get; set; } = new HashSet<HinhAnh>(); // ✅ Tránh lỗi null
    }
}
