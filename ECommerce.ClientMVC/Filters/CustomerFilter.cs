using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ecommerce.ClientMVC.Interface;

namespace Ecommerce.ClientMVC.Filters
{
    public class CustomerFilter : ActionFilterAttribute
    {
        private readonly IAuthService _authService;

        public CustomerFilter(IAuthService authService)
        {
            _authService = authService;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool isAuthenticated = await _authService.IsAuthenticated();
            if (!isAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            await next();
        }
    }
}