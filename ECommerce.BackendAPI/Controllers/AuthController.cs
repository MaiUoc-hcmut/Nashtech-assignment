using Ecommerce.SharedViewModel.Responses;
using Ecommerce.SharedViewModel.ParametersType;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.BackendAPI.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.BackendAPI.Interfaces.Helper;
using Ecommerce.BackendAPI.FiltersAction;


namespace ECommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly AuthService _authService;
        private readonly IMapper _mapper;
        private readonly IDependMethod _dependMethod;

        public AuthController
        (
            IAuthRepository authRepository, 
            AuthService authService, 
            IMapper mapper,
            IDependMethod dependMethod
        )
        
        {
            _authRepository = authRepository;
            _authService = authService;
            _mapper = mapper;
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

                var customerDTO = _mapper.Map<CustomerDTO>(registeredUser);
                var response = new RegisterResponse
                {
                    Success = true,
                    Message = "Registration successful.",
                    Customer = customerDTO
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
                Customer = _mapper.Map<CustomerDTO>(customer)
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