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

        public async Task<(int TotalReviews, IEnumerable<Review> Reviews)> GetReviewsAsync
        (
            List<int> productIds,
            int pageNumber,
            double minRating,
            double maxRating,
            DateTime? startDate,
            DateTime? endDate,
            string sortBy,
            bool isAsc
        )
        {
            var pageSize = 10;

            // Build the query
            var query = _context.Reviews
                .Include(r => r.Product)
                .Include(r => r.Customer)
                .Where(r => r.Rating >= minRating && r.Rating <= maxRating);

            // Filter by product IDs if provided
            if (productIds != null && productIds.Any())
            {
                query = query.Where(r => productIds.Contains(r.Product.Id));
            }

            // Filter by date range if provided
            if (startDate.HasValue)
            {
                query = query.Where(r => r.CreatedAt >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query = query.Where(r => r.CreatedAt <= endDate.Value);
            }

            // Get the total number of reviews
            int totalReviews = await query.CountAsync();

            // Apply sorting
            query = isAsc
                ? query.OrderBy(r => EF.Property<object>(r, sortBy))
                : query.OrderByDescending(r => EF.Property<object>(r, sortBy));

            // Apply pagination
            var reviews = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Return both total reviews and the paginated list
            return (totalReviews, reviews);
        }

        public async Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId)
        {
            return await _context.Reviews
                .Include(r => r.Customer)
                .Where(r => r.Product.Id == productId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsOfCustomerAsync(int customerId)
        {
            return await _context.Reviews
                .Include(r => r.Product)
                .Where(r => r.Customer.Id == customerId)
                .Select(r => new Review
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Text = r.Text,
                    CreatedAt = r.CreatedAt,
                    Product = new Product
                    {
                        Id = r.Product.Id,
                        Name = r.Product.Name,
                        Description = r.Product.Description,
                        ImageUrl = r.Product.ImageUrl
                    }
                })
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Review?> GetReviewAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.Customer)
                .Include(r => r.Product) 
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Review> AddReviewAsync(Review review)
        {
            var response = await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return response.Entity;
        }

        public async Task<bool> UpdateReviewAsync(Review review)
        {
            _context.Reviews.Update(review);
            return await SaveAsync();
        }

        public async Task<bool> DeleteReviewAsync(int id)
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