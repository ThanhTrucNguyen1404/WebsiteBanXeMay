using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteBanXeMay___Coding.Models
{
    public class NhanVien
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string HoTen { get; set; }

        public string? ChucVu { get; set; }

        // Liên kết với ApplicationUser (Identity)
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

        // Thêm trường Email để lấy từ ApplicationUser
        [NotMapped] // Không lưu vào database, chỉ dùng để hiển thị
        public string? Email => User?.Email;

        // Danh sách phiếu nhập do nhân viên này tạo
        public virtual ICollection<PhieuNhap>? PhieuNhaps { get; set; }
    }
}
