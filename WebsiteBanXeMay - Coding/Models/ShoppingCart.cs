using System.Collections.Generic;
using System.Linq;

namespace WebsiteBanXeMay___Coding.Models
{
    public class ShoppingCart
    {
        public List<GioHangItem> Items { get; set; } = new List<GioHangItem>();

        // 🔹 Tính tổng giá trị giỏ hàng
        public decimal TotalPrice()
        {
            return Items.Sum(item => item.Price * item.Quantity);
        }

        // 🔹 Thêm sản phẩm vào giỏ hàng
        public void AddItem(GioHangItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }

        // 🔹 Xóa sản phẩm khỏi giỏ hàng
        public void RemoveItem(int productId)
        {
            Items.RemoveAll(i => i.ProductId == productId);
        }
    }
}
