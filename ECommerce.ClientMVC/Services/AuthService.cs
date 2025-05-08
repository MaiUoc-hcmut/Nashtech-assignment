using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.ParametersType;
using Ecommerce.SharedViewModel.Responses;
using Ecommerce.ClientMVC.Interface;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.ClientMVC.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _apiBaseUrl;

        public AuthService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _apiBaseUrl = "http://localhost:5113";
        }

        public async Task<LoginResponse> LoginAsync(LoginParameter model)
        {
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync($"{_apiBaseUrl}/api/Auth/login", content);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponse>() ?? new LoginResponse 
                { 
                    Success = false, 
                    Message = "Unexpected error occurred while processing the login response." 
                };
            }
            
            return new LoginResponse 
            { 
                Success = false, 
                Message = "Login failed. Please check your credentials." 
            };
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterParameter model)
        {
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync($"{_apiBaseUrl}/api/Auth/register", content);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RegisterResponse>() ?? new RegisterResponse 
                { 
                    Success = false, 
                    Message = "Unexpected error occurred while processing the registration response." 
                };
            }
            
            return new RegisterResponse 
            { 
                Success = false, 
                Message = "Registration failed. Please try again." 
            };
        }

        public void StoreUserData(LoginResponse response)
        {
            // Store user info in session (equivalent to localStorage in browser)
            var userInfoJson = JsonSerializer.Serialize(response.Customer);
            if (_httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Session.SetString("UserInfo", userInfoJson);
            }
        }

        public CustomerResponse? GetCurrentUser()
        {
            var userInfoJson = _httpContextAccessor.HttpContext?.Session?.GetString("UserInfo");
            
            if (!string.IsNullOrEmpty(userInfoJson))
            {
                return JsonSerializer.Deserialize<CustomerResponse>(userInfoJson);
            }
            
            return null;
        }

        public async Task<bool> IsAuthenticated()
        {
            var client = _httpClientFactory.CreateClient();
            bool IsAuthenticated = await client.GetFromJsonAsync<bool>($"{_apiBaseUrl}/api/Auth/customer/validate");
            return IsAuthenticated;
        }
        
        public async Task<bool> Logout()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsync($"{_apiBaseUrl}/api/Auth/logout", null);

                if (response.IsSuccessStatusCode)
                {
                    // Clear session
                    if (_httpContextAccessor.HttpContext?.Session != null)
                    {
                        _httpContextAccessor.HttpContext.Session.Remove("UserInfo");
                    }
                    return true;
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Error: {response.StatusCode}, Content: {errorContent}");

                return false; // Logout failed
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during logout: {ex.Message}");
                return false; // Logout failed
            }
        }
    }
}