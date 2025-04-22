using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.ParametersType;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace Ecommerce.BackendAPI.FiltersAction
{
    public class GetOrderFilter : ActionFilterAttribute
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAdminRepository _adminRepository; 

        public GetOrderFilter
        (
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            IAdminRepository adminRepository
        )
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _adminRepository = adminRepository;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var orderId = context.RouteData.Values["orderId"]?.ToString();
            var customerId = context.RouteData.Values["customerId"]?.ToString();
            var productId = context.RouteData.Values["productId"]?.ToString();

            var userId = httpContext.Items["UserId"]?.ToString();

            if (userId == null || orderId == null && customerId == null && productId == null)
            {
                context.Result = new BadRequestObjectResult(new { Error = "Invalid ID" });
                return;
            }

            var admin = _adminRepository.GetAdminById(int.Parse(userId));

            if (orderId != null) {
                var order = await _orderRepository.GetOrderById(int.Parse(orderId));
                if (order == null)
                {
                    context.Result = new NotFoundObjectResult(new { Error = "Order not found" });
                    return;
                }

                if (order.Customer.Id != int.Parse(userId) && admin == null)
                {
                    context.Result = new UnauthorizedObjectResult(new { Error = "Unauthorized access to order" });
                    return;
                }
            } else if (customerId != null) {
                var customer = await _customerRepository.GetCustomerById(int.Parse(customerId));
                if (customer == null)
                {
                    context.Result = new NotFoundObjectResult(new { Error = "Customer not found" });
                    return;
                }

                if (customer.Id != int.Parse(userId) && admin == null)
                {
                    context.Result = new UnauthorizedObjectResult(new { Error = "Unauthorized access to customer" });
                    return;
                }
            } else if (productId != null) {
                var product = await _orderRepository.GetOrdersOfProduct(int.Parse(productId));
                if (product == null)
                {
                    context.Result = new NotFoundObjectResult(new { Error = "Product not found" });
                    return;
                }

                if (admin == null)
                {
                    context.Result = new UnauthorizedObjectResult(new { Error = "Unauthorized access to product" });
                    return;
                }
            }

            await next(); // Proceed to the action method
        }
    }

    public class CreateOrderFilter : ActionFilterAttribute
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IVariantRepository _variantRepository;

        public CreateOrderFilter (ICustomerRepository customerRepository, IVariantRepository variantRepository)
        {
            _customerRepository = customerRepository;
            _variantRepository = variantRepository;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;

            var customerId = httpContext.Items["UserId"]?.ToString();
            var customer = customerId != null 
                ? await _customerRepository.GetCustomerById(int.Parse(customerId)) 
                : null;
            if (customer == null) {
                context.Result = new BadRequestObjectResult(new { Error = "Invalid customer" });
                return;
            }

            httpContext.Request.EnableBuffering();

            using var reader = new StreamReader(httpContext.Request.Body);
            var bodyContent = await reader.ReadToEndAsync();
            httpContext.Request.Body.Position = 0;

            var bodyRequest = JsonSerializer.Deserialize<CreateOrderParameter>(bodyContent);
            if (bodyRequest == null || bodyRequest.Variants == null || bodyRequest.Variants.Count == 0) {
                context.Result = new BadRequestObjectResult(new { Error = "Invalid body of request" });
                return;
            }

            var variantList = new List<Variant>();

            foreach (var id in bodyRequest.Variants) {
                var variant = await _variantRepository.GetVariantById(id);
                if (variant == null) {
                    context.Result = new BadRequestObjectResult(new { Error = $"Not found variant with ID = {id}" });
                    return;
                }
                variantList.Add(variant);
            }

            httpContext.Items["Customer"] = customer;
            httpContext.Items["Variants"] = variantList;
            await next();
        }
    }
}