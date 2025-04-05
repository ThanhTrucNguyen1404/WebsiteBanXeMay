using System.ComponentModel.DataAnnotations;

namespace WebsiteBanXeMay___Coding.Models
{
    public class LoaiThanhVien
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string? TenLoai { get; set; } // 🔥 Cho phép null nếu chưa có tên

        [MaxLength(255)]
        public string? MoTa { get; set; } // 🔥 Cho phép null nếu không có mô tả

        // Liên kết đến danh sách thành viên thuộc loại này
        public virtual ICollection<ThanhVien> ThanhViens { get; set; } = new List<ThanhVien>();

        // Liên kết với bảng quyền (nhiều - nhiều)
        public virtual ICollection<LoaiThanhVien_Quyen> LoaiThanhVien_Quyens { get; set; } = new List<LoaiThanhVien_Quyen>();
    }
}
