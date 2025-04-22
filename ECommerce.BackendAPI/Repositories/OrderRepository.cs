using Ecommerce.SharedViewModel.Models;
using Ecommerce.BackendAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Ecommerce.BackendAPI.Data;


namespace Ecommerce.BackendAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;

        public OrderRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order?> GetOrderById(int orderId)
        {
            return await _context.Orders
                .Include(o => o.VariantOrders)
                .ThenInclude(vo => vo.Variant)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrderByUserId(int customerId)
        {
            return await _context.Orders
                .Include(o => o.VariantOrders)
                .ThenInclude(vo => vo.Variant)
                .Where(o => o.Customer.Id == customerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersOfProduct(int productId)
        {
            return await _context.Orders
                .Include(o => o.VariantOrders)
                .ThenInclude(vo => vo.Variant)
                .Where(o => o.VariantOrders.Any(vo => vo.Variant.Product.Id == productId))
                .ToListAsync();
        }

        public async Task<bool> CreateOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
            return await Save();
        }

        public async Task<bool> DeleteOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            _context.Orders.Remove(order);
            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}