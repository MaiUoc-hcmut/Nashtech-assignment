using Ecommerce.SharedViewModel.Responses;

namespace Ecommerce.ClientMVC.Interface
{
    public interface ICartService
    {
        Task<CartItemResponse> GetCartOfCustomer(int customerId);
        Task<bool> AddToCart(int cartId, int variantId, int quantity = 1);
        Task<bool> DeleteFromCart(int cartId, int variantId);
        // Task<Result> UpdateCartItemQuantity(int productId, int quantity);
        // Task<Result> ClearCart();
    }
}