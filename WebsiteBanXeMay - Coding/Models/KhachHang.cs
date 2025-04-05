using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace WebsiteBanXeMay___Coding.Models
{
    public class KhachHang
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string? HoTen { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; }

        [MaxLength(15)]
        [Phone]
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Số điện thoại không hợp lệ!")]
        public string? SoDienThoai { get; set; } // 🔥 Chỉ nhận số và dấu `+`, từ 10-15 ký tự

        [MaxLength(255)]
        public string? DiaChi { get; set; }

        public DateTime? NgayTao { get; set; } = DateTime.Now;

        // Thêm UserId để liên kết với bảng AspNetUsers (tài khoản người dùng)
        public string UserId { get; set; }

        // Liên kết với bảng DonDatHang (nếu có)
        public virtual ICollection<DonDatHang> DonDatHangs { get; set; } = new List<DonDatHang>();
    }
}
