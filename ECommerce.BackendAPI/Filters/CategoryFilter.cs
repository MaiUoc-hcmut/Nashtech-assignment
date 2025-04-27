using Ecommerce.BackendAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace Ecommerce.BackendAPI.FiltersAction
{
    public class CategoryAndParentAndClassificationFilter : ActionFilterAttribute
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IClassificationRepository _classificationRepository;

        public CategoryAndParentAndClassificationFilter(ICategoryRepository categoryRepository, IClassificationRepository classificationRepository)
        {
            _categoryRepository = categoryRepository;
            _classificationRepository = classificationRepository;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var Id = context.RouteData.Values["Id"]?.ToString();
            var isCategoryEndpoint = httpContext.Request.Path.Value?.Contains("Category", StringComparison.OrdinalIgnoreCase) ?? false;
            var isClassificationEndpoint = httpContext.Request.Path.Value?.Contains("Classification", StringComparison.OrdinalIgnoreCase) ?? false;

            Console.WriteLine(Id);
            Console.WriteLine(isClassificationEndpoint);

            if (string.IsNullOrEmpty(Id))
            {
                context.Result = new BadRequestObjectResult(new { Error = "Id is null or empty" });
                return;
            }

            if (isCategoryEndpoint)
            {
                var category = await _categoryRepository.GetCategoryById(int.Parse(Id));
                if (category == null) 
                {
                    context.Result = new BadRequestObjectResult(new { Error = "Category not found" });
                    return;
                }

                httpContext.Items["Category"] = category;
            }
            else if (isClassificationEndpoint)
            {
                var classification = await _classificationRepository.GetClassificationById(int.Parse(Id));
                if (classification == null) 
                {
                    context.Result = new BadRequestObjectResult(new { Error = "Classification not found" });
                    return;
                }

                httpContext.Items["Classification"] = classification;
            }

            await next(); 
        }
    }
}