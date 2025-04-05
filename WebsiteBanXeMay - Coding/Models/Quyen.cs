using System.ComponentModel.DataAnnotations;

namespace WebsiteBanXeMay___Coding.Models
{
    public class Quyen
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)] // 🔥 Hạn chế độ dài tên quyền
        public string TenQuyen { get; set; }

        [MaxLength(255)] // 🔥 Hạn chế độ dài mô tả
        public string? MoTa { get; set; }

        // Liên kết với bảng trung gian LoaiThanhVien_Quyen (Nhiều - Nhiều)
        public virtual ICollection<LoaiThanhVien_Quyen> LoaiThanhVien_Quyens { get; set; } = new List<LoaiThanhVien_Quyen>();
    }
}
