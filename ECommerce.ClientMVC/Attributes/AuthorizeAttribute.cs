using Ecommerce.ClientMVC.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ecommerce.ClientMVC.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var authService = context.HttpContext.RequestServices.GetService<IAuthService>();
            
            if (authService == null || !await authService.IsAuthenticatedAsync())
            {
                // User is not authenticated, redirect to login
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}