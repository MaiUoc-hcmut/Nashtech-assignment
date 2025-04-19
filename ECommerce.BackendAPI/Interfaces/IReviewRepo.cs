using Ecommerce.SharedViewModel.Models;

namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetReviewsByProductId(int productId);
        Task<Review?> GetReview(int id);
        Task<Review> AddReview(Review review);
        Task<bool> UpdateReview(Review review);
        Task<bool> DeleteReview(int id);
        Task<bool> SaveAsync();
    }
}