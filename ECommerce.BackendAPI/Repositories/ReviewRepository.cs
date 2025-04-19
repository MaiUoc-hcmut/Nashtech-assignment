using Ecommerce.SharedViewModel.Models;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.BackendAPI.Data;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce.BackendAPI.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetReviewsByProductId(int productId)
        {
            return await _context.Reviews.Where(r => r.Product.Id == productId).ToListAsync();
        }

        public async Task<Review?> GetReview(int id)
        {
            return await _context.Reviews.FindAsync(id);
        }

        public async Task<Review> AddReview(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> UpdateReview(Review review)
        {
            _context.Reviews.Update(review);
            return await SaveAsync();
        }

        public async Task<bool> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}