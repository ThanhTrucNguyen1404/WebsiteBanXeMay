using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteBanXeMay___Coding.Models
{
    public class HinhAnh
    {
        [Key]
        public int Id { get; set; }

        [StringLength(500)]
        public string Url { get; set; } = string.Empty; // 🔥 Cho phép null nếu chưa có đường dẫn ảnh

        // ✅ Liên kết đến SanPham
        public int? SanPhamId { get; set; }
        public virtual SanPham? SanPham { get; set; }

        // ✅ Liên kết đến ChiTietDonDatHang
        public int? ChiTietDonDatHangId { get; set; }
        public virtual ChiTietDonDatHang? ChiTietDonDatHang { get; set; }

        // ✅ Liên kết đến ChiTietPhieuNhap
        public int? ChiTietPhieuNhapId { get; set; }
        public virtual ChiTietPhieuNhap? ChiTietPhieuNhap { get; set; }

        // ✅ Đánh dấu hình ảnh chính
        public bool LaHinhAnhChinh { get; set; } = false;
    }
}
