using Ecommerce.SharedViewModel.Responses;

namespace Ecommerce.ClientMVC.Interface
{
    public interface ICartService
    {
        Task<CartItemResponse> GetCartOfCustomerAsync(int customerId);
        Task<bool> AddToCartAsync(int cartId, int variantId, int quantity = 1);
        Task<bool> DeleteFromCartAsync(int cartId, int variantId);
        // Task<Result> UpdateCartItemQuantity(int productId, int quantity);
        // Task<Result> ClearCart();
    }
}