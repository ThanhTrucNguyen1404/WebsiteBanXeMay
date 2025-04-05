using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteBanXeMay___Coding.Models
{
    [Table("LichSuMuaHang")]
    public class LichSuMuaHang
    {
        [Key]
        public int Id { get; set; }

        public int? KhachHangId { get; set; }
        [ForeignKey("KhachHangId")]
        public virtual KhachHang? KhachHang { get; set; }

        [Required]
        public DateTime NgayMua { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TongTien { get; set; }

        [Required]
        [MaxLength(100)]
        public string PhuongThucThanhToan { get; set; }

        [MaxLength(500)]
        public string? DiaChiGiaoHang { get; set; }

        // 📌 Danh sách sản phẩm trong giỏ hàng đã thanh toán
        public virtual ICollection<GioHangItem> GioHangItems { get; set; } = new List<GioHangItem>();

        // 📌 Danh sách hình ảnh của sản phẩm đã mua
        public string Status { get; set; } // Added Status property

        public string OrderId { get; set; } // Add this line


    }
}
