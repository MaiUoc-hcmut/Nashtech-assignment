using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ecommerce.BackendAPI.Services;
using Ecommerce.SharedViewModel.Models;

namespace Ecommerce.BackendAPI.FiltersAction
{
    public class UpdateAndDeleteProductFilter : ActionFilterAttribute
    {
        private readonly AuthService _authService;
        public UpdateAndDeleteProductFilter(AuthService authService)
        {
            _authService = authService;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var admin = httpContext.Items["admin"] as Admin;
            var password = httpContext.Request.Cookies["password"]?.ToString();

            if (admin == null || string.IsNullOrEmpty(password))
            {
                context.Result = new BadRequestObjectResult(new { Error = "You do not have authorize to do this action" });
                return;
            }

            if (!_authService.VerifyPassword(password, admin.Password))
            {
                context.Result = new BadRequestObjectResult(new { Error = "Password does not match with your account" });
                return;
            }

            await next();
        }
    }
}