using Ecommerce.SharedViewModel.Models;


namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IOrderRepository
    {
        Task<(int TotalOrders, IEnumerable<Order> Orders)> GetAllOrders(int pageNumber = 1); // For admin
        Task<Order?> GetOrderById(int orderId);
        Task<IEnumerable<Order>> GetOrderByUserId(int customerId);
        Task<IEnumerable<Order>> GetOrdersOfProduct(int productId);
        Task<bool> CreateOrder(Order order);
        // Task<bool> UpdateOrder(Order order);
        Task<bool> DeleteOrder(int orderId);
        Task<bool> Save();
    }
}