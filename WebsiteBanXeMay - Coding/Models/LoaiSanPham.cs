using System.ComponentModel.DataAnnotations;

namespace WebsiteBanXeMay___Coding.Models
{
    public class LoaiSanPham
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string? TenLoai { get; set; } // 🔥 Cho phép null

        [MaxLength(255)]
        public string? MoTa { get; set; } // 🔥 Cho phép null nếu không có mô tả

        // Liên kết đến danh sách sản phẩm thuộc loại này
        public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}
