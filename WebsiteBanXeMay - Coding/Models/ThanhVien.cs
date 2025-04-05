using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteBanXeMay___Coding.Models
{
    public class ThanhVien
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string? HoTen { get; set; } // 🔥 Không bắt buộc nhập

        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; } // 🔥 Không ép buộc phải nhập

        [StringLength(20)]
        public string? SoDienThoai { get; set; } // 🔥 Không bắt buộc

        [StringLength(255)]
        public string? MatKhau { get; set; } // 🔥 Không bắt buộc có mật khẩu

        public int? LoaiThanhVienId { get; set; } // 🔥 Không bắt buộc phải có loại thành viên

        [ForeignKey("LoaiThanhVienId")]
        public virtual LoaiThanhVien? LoaiThanhVien { get; set; } // 🔥 Tránh lỗi NullReferenceException

        public DateTime? NgayTao { get; set; } = DateTime.Now; // 🔥 Cho phép null

        public bool? TrangThai { get; set; } = true; // 🔥 Không bắt buộc
    }
}
