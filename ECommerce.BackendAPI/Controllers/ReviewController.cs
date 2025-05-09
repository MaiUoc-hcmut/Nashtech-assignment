using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.BackendAPI.FiltersAction;
using Ecommerce.SharedViewModel.Responses;
using Microsoft.AspNetCore.Mvc;


namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews
        (
            [FromQuery] List<int> productIds,
            [FromQuery] int pageNumber = 1,
            [FromQuery] double minRating = 0,
            [FromQuery] double maxRating = 5,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string sortBy = "CreatedAt",
            [FromQuery] bool isAsc = true
        )
        {
            try
            {
                startDate ??= new DateTime(2025, 1, 1, 0, 0, 0);
                endDate ??= new DateTime(2030, 12, 31, 23, 59, 59);

                var (totalReviews, reviews) = await _reviewRepository.GetReviews(
                    productIds,
                    pageNumber,
                    minRating,
                    maxRating,
                    startDate,
                    endDate,
                    sortBy,
                    isAsc
                );

                var response = new GetReviewsResponse
                {
                    TotalReviews = totalReviews,
                    Reviews = reviews.Select(r => new
                    {
                        r.Id,
                        r.Rating,
                        r.Text,
                        CreatedAt = r.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                        Customer = r.Customer.Name,
                        Product = r.Product.Name
                    })
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { Message = "An error occurred while retrieving reviews.", Error = ex.Message });
            }
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetReviewsByProductId(int productId)
        {
            var reviews = await _reviewRepository.GetReviewsByProductId(productId);

            if (!reviews.Any())
            {
                return Ok(new GetReviewsByProductResponse
                {
                    Reviews = new List<object>(), // Return an empty list for reviews
                    AverageRating = 0, // Default average rating
                    TotalReviews = 0, // No reviews
                    RatingsCount = new Dictionary<int, int>() // Empty dictionary for ratings count
                });
            }

            var averageRating = reviews.Average(r => r.Rating);
            var totalReviews = reviews.Count();
            var ratingsCount = reviews.GroupBy(r => r.Rating)
                .ToDictionary(g => g.Key, g => g.Count());

            return Ok(new GetReviewsByProductResponse
            {
                Reviews = reviews,
                AverageRating = averageRating,
                TotalReviews = totalReviews,
                RatingsCount = ratingsCount
            });
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetReviewsOfCustomer(int customerId)
        {
            var reviews = await _reviewRepository.GetReviewsOfCustomer(customerId);
            if (reviews == null)
            {
                return NotFound(new { message = "No reviews found for this customer." });
            }

            var response = reviews.Select(r => new
            {
                r.Id,
                r.Rating,
                r.Text,
                r.CreatedAt,
                Product = new
                {
                    r.Product.Id,
                    r.Product.Name,
                    r.Product.Description,
                    r.Product.ImageUrl
                }
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReview(int id)
        {
            var review = await _reviewRepository.GetReview(id);
            if (review == null) return NotFound();
            return Ok(new {
                review.Id,
                review.Rating,
                review.Text,
                CreatedAt = review.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                CustomerName = new
                {
                    review.Customer.Id,
                    review.Customer.Name,
                },
                Product = new
                {
                    review.Product.Id,
                    review.Product.Name,
                }
            });
        }

        [HttpPost("product/{productId}")]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyWhenCreateReview))]
        public async Task<IActionResult> AddReview([FromBody] ReviewDTO reviewDto)
        {
            if (reviewDto == null) return BadRequest("Review cannot be null");
            var Customer = HttpContext.Items["Customer"] as Customer ?? throw new InvalidOperationException("Customer is not available in HttpContext.");
            var Product = HttpContext.Items["Product"] as Product ?? throw new InvalidOperationException("Product is not available in HttpContext.");
            
            var review = new Review {
                Rating = reviewDto.Rating,
                Text = reviewDto.Text,
                Customer = Customer,
                Product = Product,
            };

            var createdReview = await _reviewRepository.AddReview(review);
            return Ok(createdReview);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyWhenUpdateAndDeleteReview))]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewDTO reviewDto)
        {
            if (reviewDto == null) return BadRequest("Review cannot be null");
            var review = new Review {
                Id = id,
                Rating = reviewDto.Rating,
                Text = reviewDto.Text
            };


            var updated = await _reviewRepository.UpdateReview(review);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyWhenUpdateAndDeleteReview))]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var deleted = await _reviewRepository.DeleteReview(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}