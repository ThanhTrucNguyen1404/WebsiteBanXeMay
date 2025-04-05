using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteBanXeMay___Coding.Models
{
    public class DonDatHang
    {
        [Key]
        public int DonDatHangId { get; set; }

        // 📌 Bắt buộc có khách hàng (hoặc để nullable nếu cho phép khách vãng lai)
        [Required]
        public int KhachHangId { get; set; }
        [ForeignKey("KhachHangId")]
        public virtual KhachHang KhachHang { get; set; }

        // 📌 Mặc định ngày đặt hàng là thời điểm hiện tại
        [Required]
        public DateTime NgayDat { get; set; } = DateTime.Now;

        // 📌 Tổng tiền đơn hàng
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal TongTien { get; set; }

        // 📌 Trạng thái đơn hàng (mặc định là "Chờ xác nhận")
        [Required]
        [MaxLength(50)]
        public string TrangThai { get; set; } = "Chờ xác nhận";

        // 📌 Ghi chú đơn hàng (có thể không có)
        [MaxLength(500)]
        public string? GhiChu { get; set; }

        // 📌 Danh sách chi tiết đơn hàng
        public virtual ICollection<ChiTietDonDatHang> ChiTietDonDatHangs { get; set; } = new List<ChiTietDonDatHang>();
        public string OrderId { get; set; } // ID đơn hàng
        public string Status { get; set; }
    }
}
