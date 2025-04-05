using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; // Thêm thư viện này để hỗ trợ IFormFile

namespace WebsiteBanXeMay___Coding.Models
{
    public class Xe
    {
        [Key]
        public int Id { get; set; }

        public string? TenXe { get; set; } // 🔥 Không bắt buộc nhập tên xe

        public decimal? GiaBan { get; set; } // 🔥 Cho phép null để tránh lỗi khi chưa có giá

        public int? SoLuong { get; set; } // 🔥 Không bắt buộc nhập số lượng

        public string? MoTa { get; set; } // 🔥 Không bắt buộc mô tả

        public string? HinhAnh { get; set; } // 🔥 Không ép buộc phải có hình ảnh

        [NotMapped]
        public IFormFile? ImageFile { get; set; } // 🔥 Không bắt buộc tải ảnh lên

        public int? LoaiSanPhamId { get; set; } // 🔥 Không bắt buộc liên kết loại sản phẩm
        public LoaiSanPham? LoaiSanPham { get; set; }

        public int? NhaSanXuatId { get; set; } // 🔥 Không bắt buộc liên kết nhà sản xuất
        public NhaSanXuat? NhaSanXuat { get; set; }
    }
}
