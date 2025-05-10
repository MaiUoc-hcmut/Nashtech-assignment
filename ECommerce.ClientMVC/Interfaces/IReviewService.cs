using Ecommerce.SharedViewModel.Models;

namespace Ecommerce.ClientMVC.Interface
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetReviewsOfCustomerAsync(int customerId);
        Task<Review?> CreateReviewAsync(int productId, int Rating, string Text);
    }
}