using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.BackendAPI.FiltersAction;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;


namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper)
        {
            _mapper = mapper;
            _reviewRepository = reviewRepository;
        }

        [HttpGet("/product/{productId}")]
        public async Task<IActionResult> GetReviewsByProductId(int productId)
        {
            var reviews = await _reviewRepository.GetReviewsByProductId(productId);
            return Ok(reviews);
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
            var review = _mapper.Map<Review>(reviewDto);
            review.Customer = HttpContext.Items["Customer"] as Customer ?? throw new InvalidOperationException("Customer is not available in HttpContext.");
            review.Product = HttpContext.Items["Product"] as Product ?? throw new InvalidOperationException("Product is not available in HttpContext.");

            var createdReview = await _reviewRepository.AddReview(review);
            return Ok(createdReview);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(VerifyToken))]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewDTO reviewDto)
        {
            if (reviewDto == null) return BadRequest("Review cannot be null");
            var review = _mapper.Map<Review>(reviewDto);
            review.Id = id;

            var userId = HttpContext.Items["UserId"] as string;
            if (userId == null) return Unauthorized("User not authenticated");


            var updated = await _reviewRepository.UpdateReview(review);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var deleted = await _reviewRepository.DeleteReview(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}