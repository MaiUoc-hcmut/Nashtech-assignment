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

        public async Task<Cart?> GetCartOfCustomerAsync(int customerId, bool? includeVariants = true)
        {
            if (includeVariants == true)
            {
                return await _context.Carts
                    .Include(c => c.Customer)
                    .Include(c => c.VariantCarts)
                    .ThenInclude(vc => vc.Variant)
                    .ThenInclude(v => v.Product) // Include Product
                    .Include(c => c.VariantCarts)
                    .ThenInclude(vc => vc.Variant)
                    .ThenInclude(v => v.VariantCategories)
                    .ThenInclude(vc => vc.Category)
                    .ThenInclude(c => c.ParentCategory) // Include ParentCategory for Color and Size
                    .FirstOrDefaultAsync(c => c.Customer.Id == customerId);
            }
            return await _context.Carts
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(c => c.Customer.Id == customerId);
        }

        public async Task<bool> AddToCartAsync(Cart cart, Variant variant, int quantity)
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
                Cart = cart,
                Quantity = quantity
            };

            await _context.VariantCarts.AddAsync(variantCart);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromCartAsync(Cart cart, Variant variant)
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

        public async Task<bool> ClearCartAsync(Cart cart)
        {
            var variantCarts = await _context.VariantCarts
                .Where(vc => vc.Cart.Id == cart.Id)
                .ToListAsync();

            if (variantCarts.Count == 0)
            {
                return false;
            }

            _context.VariantCarts.RemoveRange(variantCarts);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}