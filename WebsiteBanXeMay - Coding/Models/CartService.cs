using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Models
{
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartSessionKey = "Cart";

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<GioHangItem> GetCartItems()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cartJson = session.GetString(CartSessionKey);
            return cartJson != null ? JsonConvert.DeserializeObject<List<GioHangItem>>(cartJson) : new List<GioHangItem>();
        }

        public void AddToCart(SanPham product, int quantity)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(p => p.ProductId == product.Id);

            if (item == null)
            {
                cart.Add(new GioHangItem
                {
                    ProductId = product.Id,
                    Name = product.TenSanPham,
                    Price = product.GiaBan,
                    Quantity = quantity,
                    ImageUrl = product.HinhAnh,
                    StockQuantity = product.SoLuongTonKho ?? 0
                });
            }
            else
            {
                item.Quantity += quantity;
            }

            SaveCart(cart);
        }

        public void RemoveFromCart(int productId)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(p => p.ProductId == productId);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
        }

        private void SaveCart(List<GioHangItem> cart)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString(CartSessionKey, JsonConvert.SerializeObject(cart));
        }
    }
}
