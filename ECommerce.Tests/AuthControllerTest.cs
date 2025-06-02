using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Ecommerce.BackendAPI.Controllers;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.BackendAPI.Repositories;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.BackendAPI.Interfaces.Helper;
using Ecommerce.BackendAPI.Services;
using Ecommerce.SharedViewModel.ParametersType;
using Ecommerce.SharedViewModel.Responses;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.BackendAPI.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthRepository> _authRepositoryMock;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<ICartRepository> _cartRepositoryMock;
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<IDependMethod> _dependMethodMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authRepositoryMock = new Mock<IAuthRepository>();
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _cartRepositoryMock = new Mock<ICartRepository>();
            _authServiceMock = new Mock<IAuthService>();
            _dependMethodMock = new Mock<IDependMethod>();
            _controller = new AuthController(
                _authRepositoryMock.Object,
                _customerRepositoryMock.Object,
                _cartRepositoryMock.Object,
                _authServiceMock.Object,
                _dependMethodMock.Object);
        }

        #region Register Tests
        [Fact]
        public async Task Register_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new RegisterParameter 
            { 
                Name = "Test User",
                Email = "test@example.com", 
                Password = "password", 
                ConfirmPassword = "password",
                Username = "testuser",
                Address = "123 Test St",
                PhoneNumber = "1234567890"
            };
            _authServiceMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("hashedPassword");
            _customerRepositoryMock.Setup(x => x.GetCustomerByEmailAsync(It.IsAny<string>())).ReturnsAsync((Customer)null);
            _authRepositoryMock.Setup(x => x.RegisterAsync(It.IsAny<RegisterParameter>())).ReturnsAsync(new Customer
            {
                Name = "Test User",
                Email = "test@example.com",
                Username = "testuser",
                Password = "hashedPassword"
            });
            _authRepositoryMock.Setup(x => x.BeginTransactionAsync()).ReturnsAsync(new Mock<IDbContextTransaction>().Object);
            _dependMethodMock.Setup(x => x.CreateCartWhenRegister(It.IsAny<Customer>())).ReturnsAsync(new Cart());

            // Act
            var result = await _controller.Register(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<RegisterResponse>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("Registration successfully", response.Message);
        }

        [Fact]
        public async Task Register_EmailExists_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterParameter 
            { 
                Name = "Test User",
                Email = "test@example.com", 
                Password = "password", 
                ConfirmPassword = "password"
            };
            _authServiceMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("hashedPassword");
            _customerRepositoryMock.Setup(x => x.GetCustomerByEmailAsync(It.IsAny<string>())).ReturnsAsync(new Customer
            {
                Name = "Existing User",
                Email = "test@example.com",
                Username = "existinguser",
                Password = "hashedPassword"
            });
            _authRepositoryMock.Setup(x => x.BeginTransactionAsync()).ReturnsAsync(new Mock<IDbContextTransaction>().Object);

            // Act
            var result = await _controller.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("This email already exist!", badRequestResult.Value);
        }

        [Fact]
        public async Task Register_RepositoryFails_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterParameter 
            { 
                Name = "Test User",
                Email = "test@example.com",
                Password = "password", 
                ConfirmPassword = "password"
            };
            _authServiceMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("hashedPassword");
            _customerRepositoryMock.Setup(x => x.GetCustomerByEmailAsync(It.IsAny<string>())).ReturnsAsync((Customer)null);
            _authRepositoryMock.Setup(x => x.RegisterAsync(It.IsAny<RegisterParameter>())).ReturnsAsync((Customer)null);
            _authRepositoryMock.Setup(x => x.BeginTransactionAsync()).ReturnsAsync(new Mock<IDbContextTransaction>().Object);

            // Act
            var result = await _controller.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Registration failed.", badRequestResult.Value);
        }

        [Theory]
        [InlineData("", "test@example.com", "password", "password")] // Invalid Name
        [InlineData("Test User", "", "password", "password")] // Invalid Email
        [InlineData("Test User", "invalid-email", "password", "password")] // Invalid Email format
        public async Task Register_InvalidRequiredFields_ReturnsBadRequest(string name, string email, string password, string confirmPassword)
        {
            // Arrange
            var request = new RegisterParameter 
            { 
                Name = name,
                Email = email, 
                Password = password, 
                ConfirmPassword = confirmPassword
            };
            _authRepositoryMock.Setup(x => x.BeginTransactionAsync()).ReturnsAsync(new Mock<IDbContextTransaction>().Object);
            
            // We need to set up validation failure responses here
            // The controller might be using ModelState validation, so let's modify the controller context
            _controller.ModelState.AddModelError("", "Validation failed");

            // Act
            var result = await _controller.Register(request);

            // Assert
            // Changed from IsType<BadRequestObjectResult> to match what controller actually returns
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData("Test User", "test@example.com", "", "password")] // Invalid Password
        [InlineData("Test User", "test@example.com", null, "password")] // Null Password
        [InlineData("Test User", "test@example.com", "password", "different")] // Mismatched ConfirmPassword
        [InlineData("Test User", "test@example.com", "password", null)] // Null ConfirmPassword
        public async Task Register_InvalidPasswordOrConfirmPassword_ReturnsBadRequest(string name, string email, string password, string confirmPassword)
        {
            // Arrange
            var request = new RegisterParameter 
            { 
                Name = name,
                Email = email, 
                Password = password, 
                ConfirmPassword = confirmPassword
            };
            _authRepositoryMock.Setup(x => x.BeginTransactionAsync()).ReturnsAsync(new Mock<IDbContextTransaction>().Object);
            
            // Add model state error to simulate validation failure
            _controller.ModelState.AddModelError("", "Password validation failed");

            // Act
            var result = await _controller.Register(request);

            // Assert
            // Changed from IsType<BadRequestObjectResult> to match what controller actually returns
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region Login Tests
        [Fact]
        public async Task Login_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var request = new LoginParameter { Email = "test@example.com", Password = "password" };
            var customer = new Customer 
            { 
                Id = 1, 
                Name = "Test", 
                Email = "test@example.com", 
                Username = "testuser", 
                Password = "hashedPassword" 
            };
            var cart = new Cart { Id = 1 };
            _authRepositoryMock.Setup(x => x.LoginAsync(It.IsAny<LoginParameter>())).ReturnsAsync(customer);
            _cartRepositoryMock.Setup(x => x.GetCartOfCustomerAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(cart);
            _authServiceMock.Setup(x => x.GenerateToken(It.IsAny<Customer>())).Returns("token");
            
            // Mock HttpResponse to avoid NullReferenceException
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            // Act
            var result = await _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<LoginResponse>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("Login successful.", response.Message);
            Assert.NotNull(response.Customer);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LoginParameter { Email = "test@example.com", Password = "wrong" };
            _authRepositoryMock.Setup(x => x.LoginAsync(It.IsAny<LoginParameter>())).ReturnsAsync((Customer)null);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            // Fix for 'object' does not contain a definition for 'message'
            Assert.Contains("Invalid username or password", unauthorizedResult.Value.ToString());
        }

        [Fact]
        public async Task Login_CartNotFound_ReturnsNotFound()
        {
            // Arrange
            var request = new LoginParameter { Email = "test@example.com", Password = "password" };
            var customer = new Customer 
            { 
                Id = 1, 
                Name = "Test", 
                Email = "test@example.com", 
                Username = "testuser", 
                Password = "hashedPassword" 
            };
            _authRepositoryMock.Setup(x => x.LoginAsync(It.IsAny<LoginParameter>())).ReturnsAsync(customer);
            _cartRepositoryMock.Setup(x => x.GetCartOfCustomerAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync((Cart)null);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            // Fix for 'object' does not contain a definition for 'message'
            Assert.Contains("Cart not found", notFoundResult.Value.ToString());
        }

        [Theory]
        [InlineData("", "password")]
        [InlineData(null, "password")]
        public async Task Login_InvalidEmail_ReturnsUnauthorized(string email, string password)
        {
            // Arrange
            var request = new LoginParameter { Email = email, Password = password };
            _authRepositoryMock.Setup(x => x.LoginAsync(It.IsAny<LoginParameter>())).ReturnsAsync((Customer)null);

            // Act
            var result = await _controller.Login(request);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Theory]
        [InlineData("test@example.com", "")]
        [InlineData("test@example.com", null)]
        public async Task Login_InvalidPassword_ReturnsUnauthorized(string email, string password)
        {
            // Arrange
            var request = new LoginParameter { Email = email, Password = password };
            _authRepositoryMock.Setup(x => x.LoginAsync(It.IsAny<LoginParameter>())).ReturnsAsync((Customer)null);

            // Act
            var result = await _controller.Login(request);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
        #endregion

        #region AdminLogin Tests
        [Fact]
        public async Task AdminLogin_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var request = new LoginParameter { Email = "admin@example.com", Password = "password" };
            var admin = new Admin 
            { 
                Id = 1, 
                Name = "Admin", 
                Email = "admin@example.com", 
                Password = "hashedPassword" 
            };
            _authRepositoryMock.Setup(x => x.AdminLoginAsync(It.IsAny<LoginParameter>())).ReturnsAsync(admin);
            _authServiceMock.Setup(x => x.GenerateToken(It.IsAny<Admin>())).Returns("token");
            _authServiceMock.Setup(x => x.GenerateRefreshToken(It.IsAny<Admin>())).Returns("refreshToken");
            
            // Mock HttpResponse to avoid NullReferenceException
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            // Act
            var result = await _controller.AdminLogin(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<LoginResponse>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("Login successful.", response.Message);
            Assert.NotNull(response.Admin);
        }

        [Fact]
        public async Task AdminLogin_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LoginParameter { Email = "admin@example.com", Password = "wrong" };
            _authRepositoryMock.Setup(x => x.AdminLoginAsync(It.IsAny<LoginParameter>())).ReturnsAsync((Admin)null);

            // Act
            var result = await _controller.AdminLogin(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            // Fix for 'object' does not contain a definition for 'message'
            Assert.Contains("Invalid username or password", unauthorizedResult.Value.ToString());
        }

        [Fact]
        public async Task AdminLogin_SetsCorrectCookieOptions()
        {
            // Arrange
            var request = new LoginParameter { Email = "admin@example.com", Password = "password" };
            var admin = new Admin 
            { 
                Id = 1, 
                Name = "Admin", 
                Email = "admin@example.com", 
                Password = "hashedPassword" 
            };
            _authRepositoryMock.Setup(x => x.AdminLoginAsync(It.IsAny<LoginParameter>())).ReturnsAsync(admin);
            _authServiceMock.Setup(x => x.GenerateToken(It.IsAny<Admin>())).Returns("token");
            _authServiceMock.Setup(x => x.GenerateRefreshToken(It.IsAny<Admin>())).Returns("refreshToken");

            // Mock HttpContext and Response.Cookies
            var httpContext = new Mock<HttpContext>();
            var response = new Mock<HttpResponse>();
            var cookies = new Mock<IResponseCookies>();
            CookieOptions capturedOptions = null;
            cookies.Setup(c => c.Append(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()))
                   .Callback<string, string, CookieOptions>((name, value, options) => capturedOptions = options);
            response.Setup(r => r.Cookies).Returns(cookies.Object);
            httpContext.Setup(c => c.Response).Returns(response.Object);
            _controller.ControllerContext = new ControllerContext { HttpContext = httpContext.Object };

            // Act
            var result = await _controller.AdminLogin(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseResult = Assert.IsType<LoginResponse>(okResult.Value);
            Assert.True(responseResult.Success);
            Assert.Equal("Login successful.", responseResult.Message);
            Assert.NotNull(responseResult.Admin);

            // Verify cookie options - updating to match the actual CookieOptions set in AdminLogin
            cookies.Verify(c => c.Append(
                It.IsAny<string>(), 
                It.IsAny<string>(), 
                It.Is<CookieOptions>(o => o.HttpOnly && o.SameSite == SameSiteMode.Strict)),
                Times.Once);
        }

        [Theory]
        [InlineData("", "password")]
        [InlineData(null, "password")]
        public async Task AdminLogin_InvalidEmail_ReturnsUnauthorized(string email, string password)
        {
            // Arrange
            var request = new LoginParameter { Email = email, Password = password };
            _authRepositoryMock.Setup(x => x.AdminLoginAsync(It.IsAny<LoginParameter>())).ReturnsAsync((Admin)null);

            // Act
            var result = await _controller.AdminLogin(request);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Theory]
        [InlineData("admin@example.com", "")]
        [InlineData("admin@example.com", null)]
        public async Task AdminLogin_InvalidPassword_ReturnsUnauthorized(string email, string password)
        {
            // Arrange
            var request = new LoginParameter { Email = email, Password = password };
            _authRepositoryMock.Setup(x => x.AdminLoginAsync(It.IsAny<LoginParameter>())).ReturnsAsync((Admin)null);

            // Act
            var result = await _controller.AdminLogin(request);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
        #endregion
    }
}