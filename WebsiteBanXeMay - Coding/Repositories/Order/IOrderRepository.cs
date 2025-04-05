using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersByUserAsync(string userId); // Lấy đơn hàng của người dùng
        Task<Order> GetByIdAsync(string id); // Lấy đơn hàng theo ID (kiểu string)
        Task AddAsync(Order order); // Thêm đơn hàng mới
        Task UpdateAsync(Order order); // Cập nhật đơn hàng
        Task DeleteAsync(string id); // Xóa đơn hàng
    }
}
