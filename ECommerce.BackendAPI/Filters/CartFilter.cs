using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ecommerce.BackendAPI.FiltersAction
{
    public class AddToCartFilter : ActionFilterAttribute
    {
        private readonly ICartRepository _cartRepository;
        private readonly IVariantRepository _variantRepository;

        public AddToCartFilter(ICartRepository cartRepository, IVariantRepository variantRepository)
        {
            _variantRepository = variantRepository;
            _cartRepository = cartRepository;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var cartId = context.RouteData.Values["cartId"]?.ToString();
            var variantId = context.RouteData.Values["variantId"]?.ToString();
            var customerId = httpContext.Items["UserId"]?.ToString();

            if (cartId == null || variantId == null || customerId == null)
            {
                context.Result = new BadRequestObjectResult(new { Error = "Invalid cart or variant ID" });
                return;
            }

            Console.WriteLine("End");
            var cart = await _cartRepository.GetCartOfCustomer(int.Parse(cartId));
            if (cart == null)
            {
                context.Result = new NotFoundObjectResult(new { Error = "Cart not found" });
                return;
            }

            Console.WriteLine("End");
            if (cart.Customer.Id != int.Parse(customerId))
            {
                context.Result = new UnauthorizedObjectResult(new { Error = "Unauthorized access to cart" });
                return;
            }

            Console.WriteLine("End");
            var variant = await _variantRepository.GetVariantById(int.Parse(variantId));
            if (variant == null)
            {
                context.Result = new NotFoundObjectResult(new { Error = "Variant not found" });
                return;
            }

            httpContext.Items["Cart"] = cart;
            httpContext.Items["Variant"] = variant;
            Console.WriteLine("End");
            await next();
        }
    }

    public class RemoveFromCartFilter : ActionFilterAttribute
    {
        private readonly ICartRepository _cartRepository;
        private readonly IVariantRepository _variantRepository;

        public RemoveFromCartFilter(ICartRepository cartRepository, IVariantRepository variantRepository)
        {
            _cartRepository = cartRepository;
            _variantRepository = variantRepository;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var cartId = context.RouteData.Values["cartId"]?.ToString();
            var variantId = context.RouteData.Values["variantId"]?.ToString();
            var customerId = httpContext.Items["UserId"]?.ToString();

            if (cartId == null || variantId == null || customerId == null)
            {
                context.Result = new BadRequestObjectResult(new { Error = "Invalid cart or variant ID" });
                return;
            }

            var cart = await _cartRepository.GetCartOfCustomer(int.Parse(cartId));
            if (cart == null)
            {
                context.Result = new NotFoundObjectResult(new { Error = "Cart not found" });
                return;
            }

            if (cart.Customer.Id != int.Parse(customerId))
            {
                context.Result = new UnauthorizedObjectResult(new { Error = "Unauthorized access to cart" });
                return;
            }
            var variant = await _variantRepository.GetVariantById(int.Parse(variantId));
            if (variant == null)
            {
                context.Result = new NotFoundObjectResult(new { Error = "Variant not found" });
                return;
            }

            httpContext.Items["Cart"] = cart;
            httpContext.Items["Variant"] = variant;

            await next();
        }
    }
}