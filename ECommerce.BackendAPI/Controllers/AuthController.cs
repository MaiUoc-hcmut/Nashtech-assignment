using Ecommerce.SharedViewModel.Responses;
using Ecommerce.SharedViewModel.ParametersType;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.BackendAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.BackendAPI.Interfaces.Helper;
using Ecommerce.BackendAPI.FiltersAction;
using Ecommerce.SharedViewModel.Models;


namespace ECommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly AuthService _authService;
        private readonly IDependMethod _dependMethod;

        public AuthController
        (
            IAuthRepository authRepository, 
            ICustomerRepository customerRepository,
            AuthService authService, 
            IDependMethod dependMethod
        )
        
        {
            _authRepository = authRepository;
            _customerRepository = customerRepository;
            _authService = authService;
            _dependMethod = dependMethod;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterParameter request)
        {
            using var transaction = await _authRepository.BeginTransactionAsync();
            try
            {
                var hashedPassword = _authService.HashPassword(request.Password);
                request.Password = hashedPassword;

                var customer = _customerRepository.GetCustomerByEmail(request.Email);
                if (customer != null) 
                {
                    return BadRequest("This email already exist!");
                }

                var registeredUser = await _authRepository.Register(request);

                if (registeredUser == null)
                {
                    return BadRequest("Registration failed.");
                }

                var response = new RegisterResponse
                {
                    Success = true,
                    Message = "Registration successful.",
                    Customer = {
                        Id = registeredUser.Id,
                        Name = registeredUser.Name,
                        Email = registeredUser.Email,
                        Username = registeredUser.Username,
                        PhoneNumber = registeredUser.PhoneNumber,
                        Address = registeredUser.Address
                    }
                };

                await _dependMethod.CreateCartWhenRegister(registeredUser);
                await transaction.CommitAsync();

                return Ok(response);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = "An error occurred during registration.", error = e.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginParameter request)
        {
            var customer = await _authRepository.Login(request);

            if (customer == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            var token = _authService.GenerateToken(customer);

            var response = new LoginResponse
            {
                Success = true,
                Message = "Login successful.",
                Customer = customer != null ? new CustomerDTO
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    Username = customer.Username,
                    PhoneNumber = customer.PhoneNumber,
                    Address = customer.Address
                } : null
            };

            Response.Cookies.Append("access_token", token, new CookieOptions {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                MaxAge = TimeSpan.FromHours(1)
            });

            return Ok(response);
        }


        [HttpPost("admin/login")]
        public async Task<IActionResult> AdminLogin([FromBody] LoginParameter request)
        {
            var admin = await _authRepository.AdminLogin(request);

            if (admin == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            var token = _authService.GenerateToken(admin);

            var response = new LoginResponse
            {
                Success = true,
                Message = "Login successful.",
                Admin = admin != null ? new AdminDTO {
                    Id = admin.Id,
                    Name = admin.Name,
                    Email = admin.Email,
                    Password = admin.Password,
                    PhoneNumber = admin.PhoneNumber,
                    Address = admin.Address
                } : null
            };

            Response.Cookies.Append("access_token", token, new CookieOptions {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                MaxAge = TimeSpan.FromHours(1)
            });

            return Ok(response);
        }

        // [HttpPost("refresh-token")]
        // public IActionResult RefreshToken([FromBody] RefreshTokenParameter request)
        // {
        //     // Validate the refresh token and generate a new access token
        //     // This is a placeholder implementation; you should implement your own logic here.
        //     var newAccessToken = _authService.GenerateToken(new Customer { Id = request.CustomerId, Username = request.Username });

        //     var response = new RefreshTokenResponse
        //     {
        //         Success = true,
        //         Message = "Token refreshed successfully.",
        //         Token = newAccessToken
        //     };

        //     return Ok(response);
        // }
    

        [HttpGet("validate")]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyAdmin))]
        public IActionResult CheckAuthState()
        {
            var isAuthenticated = HttpContext.Items["IsAuthenticated"] == null ? 
                false : (HttpContext.Items["IsAuthenticated"] as bool?) ?? false;

            if (!isAuthenticated)
            {
                return Unauthorized("User is not authenticated");
            }

            var admin = HttpContext.Items["admin"] as Admin;
            if (admin == null)
            {
                return Unauthorized("Include token but admin is invalid");
            }
            
            return Ok(admin);
        }
    }
}