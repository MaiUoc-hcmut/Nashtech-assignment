using Ecommerce.SharedViewModel.Models;

namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IReviewRepository
    {
        Task<(int TotalReviews, IEnumerable<Review> Reviews)> GetReviews
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
        Task<IEnumerable<Review>> GetReviewsByProductId(int productId);
        Task<IEnumerable<Review>> GetReviewsOfCustomer(int customerId);
        Task<Review?> GetReview(int id);
        Task<Review> AddReview(Review review);
        Task<bool> UpdateReview(Review review);
        Task<bool> DeleteReview(int id);
        Task<bool> SaveAsync();
    }
}