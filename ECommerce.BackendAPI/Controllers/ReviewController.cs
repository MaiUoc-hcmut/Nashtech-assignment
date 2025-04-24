using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.BackendAPI.FiltersAction;
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

        [HttpGet("/product/{productId}")]
        public async Task<IActionResult> GetReviewsByProductId(int productId)
        {
            var reviews = await _reviewRepository.GetReviewsByProductId(productId);
            var averageRating = reviews.Average(r => r.Rating);
            var totalReviews = reviews.Count();
            var ratingsCount = reviews.GroupBy(r => r.Rating)
                .ToDictionary(g => g.Key, g => g.Count());
            return Ok(new {
                Reviews = reviews,
                AverageRating = averageRating,
                TotalReviews = totalReviews,
                RatingsCount = ratingsCount
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReview(int id)
        {
            var review = await _reviewRepository.GetReview(id);
            if (review == null) return NotFound();
            return Ok(review);
        }

        [HttpPost("/product/{productId}")]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyWhenCreateReview))]
        public async Task<IActionResult> AddReview([FromBody] ReviewDTO reviewDto)
        {
            if (reviewDto == null) return BadRequest("Review cannot be null");
            var review = new Review {
                Rating = reviewDto.Rating,
                Text = reviewDto.Text
            };
            review.Customer = HttpContext.Items["Customer"] as Customer ?? throw new InvalidOperationException("Customer is not available in HttpContext.");
            review.Product = HttpContext.Items["Product"] as Product ?? throw new InvalidOperationException("Product is not available in HttpContext.");

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