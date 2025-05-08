using Ecommerce.ClientMVC.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Ecommerce.ClientMVC.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Resolve the scoped service from the request service provider
            var authService = context.RequestServices.GetRequiredService<IAuthService>();
            
            if (await authService.IsAuthenticated())
            {
                var user = authService.GetCurrentUser();
                context.Items["CurrentUser"] = user;
            }

            await _next(context);
        }
    }

    // Extension method for middleware
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}