using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.ParametersType;

namespace Ecommerce.BackendAPI.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartOfCustomer(int cartId, bool? includeVariants = true);
        Task<bool> AddToCart(Cart cart, Variant variant);
        Task<bool> RemoveFromCart(Cart cart, Variant variant);
        // Task<bool> UpdateCartItem(Cart cart);
        // Task<IEnumerable<Cart>> GetAllCarts();
    }
}