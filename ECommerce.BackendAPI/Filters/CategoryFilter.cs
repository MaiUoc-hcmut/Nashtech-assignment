using Ecommerce.BackendAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace Ecommerce.BackendAPI.FiltersAction
{
    public class CategoryAndParentFilter : ActionFilterAttribute
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAdminRepository _adminRepository;

        public CategoryAndParentFilter(ICategoryRepository categoryRepository, IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
            _categoryRepository = categoryRepository;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var UserId = httpContext.Items["UserId"]?.ToString();
            if (UserId == null)
            {
                context.Result = new UnauthorizedObjectResult(new { Error = "User not authenticated" });
                return;
            }

            var admin = await _adminRepository.GetAdminById(int.Parse(UserId));
            if (admin == null)
            {
                context.Result = new UnauthorizedObjectResult(new { Error = "User not authenticated" });
                return;
            }

            await next(); 
        }
    }
}