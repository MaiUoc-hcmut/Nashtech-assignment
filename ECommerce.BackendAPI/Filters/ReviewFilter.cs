using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Ecommerce.BackendAPI.Interfaces;


namespace Ecommerce.BackendAPI.FiltersAction
{
    public class VerifyWhenCreateReview : ActionFilterAttribute
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public VerifyWhenCreateReview
        (
            ICustomerRepository customerRepository,
            IProductRepository productRepository
        )
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var userId = int.Parse(httpContext.Items["UserId"]?.ToString() ?? "0");
            var productId = int.Parse(httpContext.Request.RouteValues["productId"]?.ToString() ?? "0");

            var customer = await _customerRepository.GetCustomerByIdAsync(userId);
            if (customer == null)
            {
                context.Result = new UnauthorizedObjectResult(new { Error = "User not authenticated" });
                return;
            }

            var product = await _productRepository.GetProductByIdAsync(productId, false);
            if (product == null)
            {
                context.Result = new NotFoundObjectResult(new { Error = "Product not found" });
                return;
            }

            httpContext.Items["Product"] = product;
            httpContext.Items["Customer"] = customer;

            await next();
        }
    }

    public class VerifyWhenUpdateAndDeleteReview : ActionFilterAttribute
    {
        private readonly IReviewRepository _reviewRepository;

        public VerifyWhenUpdateAndDeleteReview(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;

            var reviewId = int.Parse(httpContext.Request.RouteValues["id"]?.ToString() ?? "0");
            var review = await _reviewRepository.GetReviewAsync(reviewId);
            if (review == null)
            {
                context.Result = new NotFoundObjectResult(new { Error = "Review not found" });
                return;
            }

            var userId = int.Parse(httpContext.Items["UserId"]?.ToString() ?? "0");
            if (review.Customer.Id != userId)
            {
                context.Result = new UnauthorizedObjectResult(new { Error = "User not authorized to update or delete this review" });
                return;
            }

            await next();
        }
    }
}