using Ecommerce.SharedViewModel.Models;

namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetReviews
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
        Task<Review?> GetReview(int id);
        Task<Review> AddReview(Review review);
        Task<bool> UpdateReview(Review review);
        Task<bool> DeleteReview(int id);
        Task<bool> SaveAsync();
    }
}