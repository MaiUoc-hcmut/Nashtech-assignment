using Ecommerce.SharedViewModel.Models;

namespace Ecommerce.ClientMVC.Interface
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetReviewsOfCustomer(int customerId);
        Task<Review?> CreateReview(int productId, int Rating, string Text);
    }
}