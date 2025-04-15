using Ecommerce.SharedViewModel.Models;
using Ecommerce.BackendAPI.Data;
using Ecommerce.BackendAPI.Interfaces.Helper;


namespace Ecommerce.BackendAPI.Helper
{
    public class DependMethod : IDependMethod
    {
        private readonly DataContext _context;

        public DependMethod(DataContext context)
        {
            _context = context;
        }

        public async Task<Cart> CreateCartWhenRegister(Customer customer)
        {
            var cart = new Cart
            {
                Customer = customer
            };

            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return cart;
        }
    }
}