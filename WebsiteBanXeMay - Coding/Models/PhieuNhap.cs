using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteBanXeMay___Coding.Models
{
    [Table("PhieuNhaps")]
    public class PhieuNhap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nhà cung cấp không được để trống.")]
        public int NhaCungCapId { get; set; }

        [ForeignKey(nameof(NhaCungCapId))]  // ✅ Đúng cách
        public virtual NhaCungCap? NhaCungCap { get; set; }

        [Required(ErrorMessage = "Nhân viên nhập hàng không được để trống.")]
        public string NhanVienId { get; set; }

        [ForeignKey(nameof(NhanVienId))] // ✅ Không có "NhanVienId1"
        public virtual ApplicationUser? NhanVien { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime NgayNhap { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Tổng tiền không được để trống.")]
        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền phải lớn hơn hoặc bằng 0.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TongTien { get; set; }

        public bool TrangThai { get; set; } = true;
    }
}
