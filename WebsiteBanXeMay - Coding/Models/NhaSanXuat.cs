using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteBanXeMay___Coding.Models
{
    public class NhaSanXuat
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [StringLength(255)]
        public string? TenNhaSanXuat { get; set; } // 🔥 Có thể null nếu chưa nhập

        [StringLength(500)]
        public string? DiaChi { get; set; }

        [StringLength(15)]
        public string? SoDienThoai { get; set; }

        [StringLength(255)]
        public string? Email { get; set; }

        [StringLength(1000)]
        public string? MoTa { get; set; }

        // Quan hệ với sản phẩm (Một nhà sản xuất có nhiều sản phẩm)
        public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}
