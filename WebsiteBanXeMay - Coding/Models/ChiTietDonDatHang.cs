using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteBanXeMay___Coding.Models
{
    public class ChiTietDonDatHang
    {
        [Key]
        public int Id { get; set; }

        // 📌 Ràng buộc với `DonDatHang`
        [Required]
        public int DonDatHangId { get; set; }

        [ForeignKey("DonDatHangId")]
        public virtual DonDatHang DonDatHang { get; set; }

        // 📌 Ràng buộc với `Xe`
        [Required]
        public int XeId { get; set; }

        [ForeignKey("XeId")]
        public virtual Xe Xe { get; set; }

        // 📌 Số lượng xe trong đơn hàng
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; }

        // 📌 Giá bán xe
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DonGia { get; set; }

        // 📌 Thành tiền = Số lượng * Giá bán
        [NotMapped]
        public decimal ThanhTien => SoLuong * DonGia;

        // 📌 Danh sách hình ảnh sản phẩm
        public virtual ICollection<HinhAnh> HinhAnhs { get; set; } = new List<HinhAnh>();

        // 📌 ID sản phẩm (Liên kết với SanPham)
        [Required]
        public int SanPhamId { get; set; }

        [ForeignKey("SanPhamId")]
        public virtual SanPham SanPham { get; set; }

        // 📌 ID đơn hàng
        [Required]
        [MaxLength(50)]
        public string OrderId { get; set; }

        // 📌 Trạng thái đơn hàng
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Chờ xác nhận";
    }
}
