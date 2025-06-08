using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.ParametersType;

namespace Ecommerce.BackendAPI.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartOfCustomerAsync(int cartId, bool? includeVariants = true);
        Task<bool> AddToCartAsync(Cart cart, Variant variant, int quantity);
        Task<bool> RemoveFromCartAsync(Cart cart, Variant variant);
        Task<bool> ClearCartAsync(Cart cart);
        // Task<bool> UpdateCartItem(Cart cart);
        // Task<IEnumerable<Cart>> GetAllCarts();
    }
}