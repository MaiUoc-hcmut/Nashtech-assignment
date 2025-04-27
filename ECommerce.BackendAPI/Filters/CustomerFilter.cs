using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Ecommerce.BackendAPI.Interfaces;


namespace Ecommerce.BackendAPI.FiltersAction
{
    public class VerifyToken : ActionFilterAttribute
    {
        private readonly IConfiguration _configuration;

        public VerifyToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var isValidateEndpoint = httpContext.Request.Path.Value?.Contains("validate", StringComparison.OrdinalIgnoreCase) ?? false;
            if (httpContext.Request.Cookies.TryGetValue("access_token", out var token))
            {
                try
                {
                    var jwtKey = _configuration["Jwt:Key"];
                    if (string.IsNullOrEmpty(jwtKey))
                    {
                        throw new InvalidOperationException("JWT key is not configured.");
                    }
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Audience"],
                        IssuerSigningKey = key
                    };  

                    var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                    var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    httpContext.Items["UserId"] = userId;
                    httpContext.Items["IsTokenValid"] = true;
                }
                catch (Exception ex)
                {
                    httpContext.Items["IsTokenValid"] = false;
                    httpContext.Items["TokenError"] = $"Invalid or expired token: {ex.Message}";
                    context.Result = new UnauthorizedObjectResult(new { Error = httpContext.Items["TokenError"] });
                    return;
                }
            }
            else
            {
                if (isValidateEndpoint)
                {
                    await next();
                }
                httpContext.Items["IsTokenValid"] = false;
                httpContext.Items["TokenError"] = "Authorization header not found";
                context.Result = new UnauthorizedObjectResult(new { Error = httpContext.Items["TokenError"] });
                return;
            }

            await next();
        }
    }

    public class CheckUserExists : ActionFilterAttribute
    {
        private readonly ICustomerRepository _customerRepository;

        public CheckUserExists(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = context.RouteData.Values["id"]?.ToString();
            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new BadRequestObjectResult(new { Error = "User ID is missing or invalid." });
                return;
            }
            var user = await _customerRepository.GetCustomerById(int.Parse(userId));
            if (user == null)
            {
                context.Result = new NotFoundObjectResult(new { Error = "User not found." });
            }

            await next();
        }
    }

}