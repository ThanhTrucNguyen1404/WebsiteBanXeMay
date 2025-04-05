using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteBanXeMay___Coding.Models
{
    public class NhaCungCap
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string TenNhaCungCap { get; set; }

        [StringLength(500)]
        public string? DiaChi { get; set; } // 🔥 Cho phép null

        [StringLength(15)]
        [RegularExpression(@"^\d{8,15}$", ErrorMessage = "Số điện thoại phải có từ 8 đến 15 chữ số.")]
        public string? SoDienThoai { get; set; } // 🔥 Chỉ nhận số, tối đa 15 ký tự

        [StringLength(255)]
        [EmailAddress]
        public string? Email { get; set; } // 🔥 Email có thể null nếu không nhập

        [StringLength(1000)] // 🔥 Giới hạn độ dài tránh lỗi
        public string? MoTa { get; set; }

        // Quan hệ với sản phẩm
        public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}
