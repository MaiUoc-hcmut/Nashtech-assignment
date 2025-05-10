using Ecommerce.SharedViewModel.Models;

namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IReviewRepository
    {
        Task<(int TotalReviews, IEnumerable<Review> Reviews)> GetReviewsAsync
        (
            List<int> productIds, 
            int pageNumber,
            double minRating,
            double maxRating,
            DateTime? startDate,
            DateTime? endDate,
            string sortBy,
            bool isAsc
        );
        Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId);
        Task<IEnumerable<Review>> GetReviewsOfCustomerAsync(int customerId);
        Task<Review?> GetReviewAsync(int id);
        Task<Review> AddReviewAsync(Review review);
        Task<bool> UpdateReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(int id);
        Task<bool> SaveAsync();
    }
}