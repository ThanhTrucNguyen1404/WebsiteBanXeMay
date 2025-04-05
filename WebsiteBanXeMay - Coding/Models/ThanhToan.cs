using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteBanXeMay___Coding.Models
{
    public class ThanhToan
    {
        [Key]
        public int Id { get; set; }

        public int? DonDatHangId { get; set; } // Cho phép null

        [ForeignKey("DonDatHangId")]
        public virtual DonDatHang? DonDatHang { get; set; }

        [StringLength(50)]
        public string? PhuongThuc { get; set; } // Không bắt buộc, có thể null

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SoTien { get; set; } // Cho phép null, không ràng buộc giá trị

        public DateTime? NgayThanhToan { get; set; } = DateTime.Now; // Cho phép null

        public string? TrangThai { get; set; } // Không ép kiểu enum, có thể để trống
    }
}
