using Ecommerce.SharedViewModel.Models;


namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IOrderRepository
    {
        Task<(int TotalOrders, IEnumerable<Order> Orders)> GetAllOrdersAsync(int pageNumber = 1); // For admin
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetOrderByUserIdAsync(int customerId);
        Task<IEnumerable<Order>> GetOrdersOfProductAsync(int productId);
        Task<bool> CreateOrderAsync(Order order);
        // Task<bool> UpdateOrder(Order order);
        Task<bool> DeleteOrderAsync(int orderId);
        Task<bool> SaveAsync();
    }
}