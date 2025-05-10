using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Ecommerce.ClientMVC.Interface;
using Ecommerce.SharedViewModel.ParametersType;
using Ecommerce.SharedViewModel.Responses;
using Ecommerce.SharedViewModel.Models;

namespace Ecommerce.ClientMVC.Services
{
    public class OrderService : IOrderService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _apiBaseUrl;

        public OrderService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _apiBaseUrl = "http://localhost:5113";
        }

        public async Task<CreateOrderResponse?> CreateOrderAsync(
            string customerName,
            string customerEmail,
            string customerPhoneNumber,
            string customerAddress,
            string variantIdList,
            int totalAmount
        )
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var httpContext = _httpContextAccessor.HttpContext;
                var apiUrl = $"{_apiBaseUrl}/api/Order";

                // Convert comma-separated variantIdList to IList<int>
                IList<int> variantIds = string.IsNullOrEmpty(variantIdList)
                    ? new List<int>()
                    : variantIdList.Split(',')
                        .Where(s => int.TryParse(s, out _))
                        .Select(int.Parse)
                        .ToList();

                // Create the request body matching CreateOrderParameter
                var orderParams = new CreateOrderParameter
                {
                    Amount = totalAmount, // Assuming totalAmount is the Amount (convert to decimal)
                    CustomerName = customerName,
                    Email = customerEmail,
                    PhoneNumber = customerPhoneNumber,
                    Address = customerAddress,
                    Variants = variantIds
                };

                // Serialize the request body to JSON
                var jsonContent = JsonSerializer.Serialize(orderParams);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Create the HTTP request
                var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
                {
                    Content = content
                };

                // Authorization
                if (httpContext?.Request.Cookies.TryGetValue("access_token", out string token) == true)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                // Send the request
                var response = await client.SendAsync(request);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    var order = JsonSerializer.Deserialize<Order>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return new CreateOrderResponse
                    {
                        Id = order.Id,
                        TotalAmount = order.Amount,
                        CreatedAt = order.CreatedAt
                    };
                }

                // Log error details if the response is not successful
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Error: {response.StatusCode}, Content: {errorContent}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in CreateOrder: {ex.Message}");
                throw;
            }
        }
    
        public async Task<IEnumerable<GetOrdersOfCustomerResponse>> GetOrdersOfCustomerAsync(int customerId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var httpContext = _httpContextAccessor.HttpContext;
                var apiUrl = $"{_apiBaseUrl}/api/Order/customer/{customerId}";

                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

                // Authorization
                if (httpContext?.Request.Cookies.TryGetValue("access_token", out string token) == true)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                // Send the request
                var response = await client.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                var orders = JsonSerializer.Deserialize<IEnumerable<GetOrdersOfCustomerResponse>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return orders ?? Enumerable.Empty<GetOrdersOfCustomerResponse>();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in CreateOrder: {ex.Message}");
                throw;
            }
        }
    
    }
}