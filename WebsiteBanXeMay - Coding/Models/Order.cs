using Microsoft.AspNetCore.Mvc.ModelBinding.Validation; // Add this line
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteBanXeMay___Coding.Models
{
    public class Order
    {
        [Key]
        public string Id { get; set; }  // Khóa chính (có thể dùng GUID hoặc tự động sinh ID nếu cần)

        [Required]
        public string UserId { get; set; }  // Mã người dùng (foreign key)

        [Required]
        public DateTime OrderDate { get; set; } // Ngày tạo đơn hàng

        [Required]
        public decimal TotalPrice { get; set; } // Tổng giá trị đơn hàng

        [Required]
        [StringLength(500)]
        public string ShippingAddress { get; set; } // Địa chỉ giao hàng

        [StringLength(1000)]
        public string Notes { get; set; } // Ghi chú đơn hàng

        // Thông tin người dùng (Foreign Key)
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        // Danh sách chi tiết đơn hàng
        [ValidateNever]
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        [Required]
        [StringLength(500)]
        public string Address { get; set; } // Địa chỉ giao hàng chi tiết

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } // Phương thức thanh toán

        [Required]
        [StringLength(50)]
        public string OrderId { get; set; } // Mã đơn hàng (đảm bảo duy nhất, có thể là GUID)

        [Required]
        [StringLength(50)]
        public string Status { get; set; } // Trạng thái đơn hàng (ví dụ: "Pending", "Completed")
    }
}
