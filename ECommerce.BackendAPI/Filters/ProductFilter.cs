using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ecommerce.BackendAPI.Services;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.BackendAPI.Interfaces;

namespace Ecommerce.BackendAPI.FiltersAction
{
    public class UpdateAndDeleteProductFilter : ActionFilterAttribute
    {
        private readonly IAuthService _authService;
        public UpdateAndDeleteProductFilter(IAuthService authService)
        {
            _authService = authService;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var admin = httpContext.Items["admin"] as Admin;

            if (admin == null)
            {
                context.Result = new BadRequestObjectResult(new { Error = "You do not have authorize to do this action" });
                return;
            }

            await next();
        }
    }
}