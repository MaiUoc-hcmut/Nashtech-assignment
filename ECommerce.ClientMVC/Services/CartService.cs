using Ecommerce.ClientMVC.Interface;
using Ecommerce.SharedViewModel.Responses;
using System.Text;
using System.Text.Json;

namespace Ecommerce.ClientMVC.Services
{
    public class CartService : ICartService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _apiBaseUrl;

        public CartService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _apiBaseUrl = "http://localhost:5113";
        }


        public async Task<CartItemResponse> GetCartOfCustomerAsync(int customerId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var httpContext = _httpContextAccessor.HttpContext;
                var apiUrl = $"{_apiBaseUrl}/api/Cart/{customerId}";
                
                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                
                // Authorization
                if (httpContext?.Request.Cookies.TryGetValue("access_token", out string token) == true)
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                
                var response = await client.SendAsync(request);
                
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to retrieve cart items. Status code: {response.StatusCode}");
                }
                
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<CartItemResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetCartItemsAsync: {ex.Message}");
                throw;
            }
        }
        
        public async Task<bool> AddToCartAsync(int cartId, int variantId, int quantity = 1)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var httpContext = _httpContextAccessor.HttpContext;
                
                // Correctly serialize just the quantity as a raw integer value
                // This matches your backend controller's [FromBody] int quantity parameter
                string jsonContent = JsonSerializer.Serialize(quantity);
                var body = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                
                
                var apiUrl = $"{_apiBaseUrl}/api/Cart/{cartId}/{variantId}";
                
                var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
                {
                    Content = body
                };
                
                // Authorization
                if (httpContext?.Request.Cookies.TryGetValue("access_token", out string token) == true)
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                
                var response = await client.SendAsync(request);
                
                Console.WriteLine($"Response status: {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API error: {errorContent}");
                    return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in AddToCart: {ex.Message}");
                return false;
            }
        }
        
        public async Task<bool> DeleteFromCartAsync(int cartId, int variantId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var httpContext = _httpContextAccessor.HttpContext;
        
                var apiUrl = $"{_apiBaseUrl}/api/Cart/{cartId}/{variantId}";
                
                var request = new HttpRequestMessage(HttpMethod.Delete, apiUrl);
                
                // Authorization
                if (httpContext?.Request.Cookies.TryGetValue("access_token", out string token) == true)
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                
                var response = await client.SendAsync(request);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API error: {errorContent}");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in AddToCart: {ex.Message}");
                return false;
            }
        }
        // Task<Result> UpdateCartItemQuantityAsync(int productId, int quantity);
    }
}