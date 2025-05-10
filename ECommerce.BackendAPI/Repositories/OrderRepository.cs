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

        public async Task<(int TotalOrders, IEnumerable<Order> Orders)> GetAllOrdersAsync(int pageNumber = 1)
        {
            int pageSize = 10;

            int totalOrders = await _context.Orders.CountAsync();

            var orders = await _context.Orders
                .Include(o => o.Customer)
                .OrderByDescending(o => o.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Return both total orders and the paginated list
            return (totalOrders, orders);
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.VariantOrders)
                .ThenInclude(vo => vo.Variant)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrderByUserIdAsync(int customerId)
        {
            return await _context.Orders
                .Include(o => o.VariantOrders)
                .ThenInclude(vo => vo.Variant)
                .ThenInclude(v => v.VariantCategories)
                .ThenInclude(vc => vc.Category)
                .ThenInclude(c => c.ParentCategory)
                .Include(o => o.VariantOrders)
                .ThenInclude(vo => vo.Variant)
                .ThenInclude(v => v.Product)
                .Where(o => o.Customer.Id == customerId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersOfProductAsync(int productId)
        {
            return await _context.Orders
                .Include(o => o.VariantOrders)
                .ThenInclude(vo => vo.Variant)
                .Where(o => o.VariantOrders.Any(vo => vo.Variant.Product.Id == productId))
                .ToListAsync();
        }

        public async Task<bool> CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            return await SaveAsync();
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            _context.Orders.Remove(order);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}