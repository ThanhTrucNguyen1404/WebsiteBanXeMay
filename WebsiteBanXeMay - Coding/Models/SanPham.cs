using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteBanXeMay___Coding.Models
{
    public class SanPham
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống!")]
        [StringLength(255)]
        public string TenSanPham { get; set; }

        public int? LoaiSanPhamId { get; set; }
        public virtual LoaiSanPham? LoaiSanPham { get; set; }

        public int? NhaSanXuatId { get; set; }
        public virtual NhaSanXuat? NhaSanXuat { get; set; }

        public int? NhaCungCapId { get; set; }
        public virtual NhaCungCap? NhaCungCap { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal GiaBan { get; set; } = 0;

        public int? SoLuongTonKho { get; set; }

        [StringLength(1000)]
        public string? MoTa { get; set; }

        public bool TrangThai { get; set; } = true;

        [StringLength(500)]
        public string? HinhAnh { get; set; }

        public virtual ICollection<GioHangItem> CartItems { get; set; } = new List<GioHangItem>();
    }
}
