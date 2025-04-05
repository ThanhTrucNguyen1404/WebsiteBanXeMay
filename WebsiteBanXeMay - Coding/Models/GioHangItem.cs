using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteBanXeMay___Coding.Models
{
    [Table("GioHangItems")]
    public class GioHangItem
    {
        [Key]
        public int Id { get; set; }

        // ID của sản phẩm
        [Required]
        public int ProductId { get; set; }

        // Tên sản phẩm
        [Required]
        public string Name { get; set; }

        // Giá của sản phẩm
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // Số lượng sản phẩm trong giỏ
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }

        // URL của ảnh sản phẩm
        public string ImageUrl { get; set; }

        // Số lượng tồn kho của sản phẩm
        public int StockQuantity { get; set; }

        // ID của sản phẩm trong bảng SanPham
        public int SanPhamId { get; set; }
        [ForeignKey("SanPhamId")]
        public virtual SanPham SanPham { get; set; }

        // Tính thành tiền cho từng món trong giỏ hàng
        [Column(TypeName = "decimal(18,2)")]
        public decimal ThanhTien => Price * Quantity;

        // Phương thức thanh toán (ví dụ: chuyển khoản, tiền mặt)
        public string PaymentMethod { get; set; }

        // ID của lịch sử mua hàng (nếu có)
        public int? LichSuMuaHangId { get; set; }
        [ForeignKey("LichSuMuaHangId")]
        public virtual LichSuMuaHang? LichSuMuaHang { get; set; }

        // ID của đơn hàng
        public string OrderId { get; set; }

        // Ngày thanh toán (nếu có)
        public DateTime? NgayThanhToan { get; set; }

        // Thêm trường trạng thái để xác định tình trạng của item trong giỏ hàng
        // Ví dụ: "Đang chờ", "Đã thanh toán", "Đã hủy"
        public string? Status { get; set; }

        // Thêm trường để lưu trữ mô tả hoặc ghi chú về item trong giỏ hàng (nếu có)
        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}
