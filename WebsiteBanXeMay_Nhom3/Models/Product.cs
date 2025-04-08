﻿using System.ComponentModel.DataAnnotations;

namespace WebsiteBanXeMay_Nhom3.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Range(100000, 100000000000, ErrorMessage = "Giá phải từ 100,000 đến 100 tỷ.")]
        public decimal Price { get; set; }

        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<ProductImage>? Images { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // 📌 Thêm số lượng sản phẩm
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng không thể âm")]
        public int Quantity { get; set; } = 0; // Mặc định số lượng là 0
    }

}
