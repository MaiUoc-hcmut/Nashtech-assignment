using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.ParametersType;
using Microsoft.EntityFrameworkCore;
using Ecommerce.BackendAPI.Data;


namespace Ecommerce.BackendAPI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DataContext _context;

        public CartRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetCartOfCustomer(int customerId, bool? includeVariants = true)
        {
            if (includeVariants == true)
            {
                return await _context.Carts
                    .Include(c => c.VariantCarts)
                    .ThenInclude(vc => vc.Variant)
                    .FirstOrDefaultAsync(c => c.Customer.Id == customerId);
            }
            return await _context.Carts.FirstOrDefaultAsync(c => c.Customer.Id == customerId);
        }

        public async Task<bool> AddToCart(Cart cart, Variant variant)
        {
            var variantCartExist = await _context.VariantCarts
                .AnyAsync(vc => vc.Cart.Id == cart.Id && vc.Variant.Id == variant.Id);

            if (variantCartExist)
            {
                return false;
            }

            var variantCart = new VariantCart
            {
                Variant = variant,
                Cart = cart
            };

            await _context.VariantCarts.AddAsync(variantCart);
            await _context.SaveChangesAsync();
            return true;
        }
    
        public async Task<bool> RemoveFromCart(Cart cart, Variant variant)
        {
            var variantCart = await _context.VariantCarts
                .FirstOrDefaultAsync(vc => vc.Cart.Id == cart.Id && vc.Variant.Id == variant.Id);

            if (variantCart == null)
            {
                return false;
            }

            _context.VariantCarts.Remove(variantCart);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}