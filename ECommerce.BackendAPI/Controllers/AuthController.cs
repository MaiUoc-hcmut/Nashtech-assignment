using Ecommerce.SharedViewModel.Responses;
using Ecommerce.SharedViewModel.ParametersType;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.BackendAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.BackendAPI.Interfaces.Helper;


namespace ECommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly AuthService _authService;
        private readonly IDependMethod _dependMethod;

        public AuthController
        (
            IAuthRepository authRepository, 
            AuthService authService, 
            IDependMethod dependMethod
        )
        
        {
            _authRepository = authRepository;
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
            var refreshToken = _authService.GenerateRefreshToken(customer);

            var response = new LoginResponse
            {
                Success = true,
                Message = "Login successful.",
                Token = token,
                RefreshToken = refreshToken,
                Customer = {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    Username = customer.Username,
                    PhoneNumber = customer.PhoneNumber,
                    Address = customer.Address
                }
            };

            return Ok(response);
        }


        [HttpPost("/admin/login")]
        public async Task<IActionResult> AdminLogin([FromBody] LoginParameter request)
        {
            var admin = await _authRepository.AdminLogin(request);

            if (admin == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            var token = _authService.GenerateToken(admin);
            var refreshToken = _authService.GenerateRefreshToken(admin);

            var response = new LoginResponse
            {
                Success = true,
                Message = "Login successful.",
                Token = token,
                RefreshToken = refreshToken,
                Admin = {
                    Id = admin.Id,
                    Name = admin.Name,
                    Email = admin.Email,
                    PhoneNumber = admin.PhoneNumber,
                    Address = admin.Address
                }
            };

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
    
    }
}