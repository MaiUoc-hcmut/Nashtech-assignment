using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Ecommerce.BackendAPI.Interfaces;

namespace Ecommerce.BackendAPI.FiltersAction
{
    public class VerifyAdmin : ActionFilterAttribute
    {
        private readonly IAdminRepository _adminrepository;

        public VerifyAdmin(IAdminRepository adminRepository)
        {
            _adminrepository = adminRepository;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;

            var Id = httpContext.Items["UserId"]?.ToString();
            if (Id == null)
            {
                context.Result = new BadRequestObjectResult(new { Error = "User ID is missing or invalid." });
                return;
            }

            var admin = await _adminrepository.GetAdminById(int.Parse(Id));
            if (admin == null) 
            {
                context.Result = new BadRequestObjectResult(new { Error = "You do not have authorize to do this action" });
                return;
            }

            await next();
        }
    }
}