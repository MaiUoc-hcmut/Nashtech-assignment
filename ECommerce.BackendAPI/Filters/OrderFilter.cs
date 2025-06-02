using Ecommerce.BackendAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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

            var admin = await _adminRepository.GetAdminByIdAsync(int.Parse(userId));

            if (orderId != null)
            {
                var order = await _orderRepository.GetOrderByIdAsync(int.Parse(orderId));
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
            }
            else if (customerId != null)
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(int.Parse(customerId));
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
            }
            else if (productId != null)
            {
                var product = await _orderRepository.GetOrdersOfProductAsync(int.Parse(productId));
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

    public class GetAllOrdersFilter : ActionFilterAttribute
    {
        private readonly IAdminRepository _adminrepository;

        public GetAllOrdersFilter(IAdminRepository adminRepository)
        {
            _adminrepository = adminRepository;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var adminId = httpContext.Items["UserId"]?.ToString();
            if (adminId == null)
            {
                context.Result = new NotFoundObjectResult(new { Error = "Invalid admin id" });
                return;
            }

            var admin = await _adminrepository.GetAdminByIdAsync(int.Parse(adminId));
            if (admin == null)
            {
                context.Result = new NotFoundObjectResult(new { Error = "Admin not found" });
                return;
            }

            await next();
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
                ? await _customerRepository.GetCustomerByIdAsync(int.Parse(customerId)) 
                : null;
            if (customer == null) {
                context.Result = new BadRequestObjectResult(new { Error = "Invalid customer" });
                return;
            }

            httpContext.Items["Customer"] = customer;
            await next();
        }
    }
}