using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Ecommerce.ClientMVC.Interface;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.ClientMVC.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _apiBaseUrl;

        public ReviewService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _apiBaseUrl = "http://localhost:5113";
        }

        public async Task<IEnumerable<Review>> GetReviewsOfCustomer(int customerId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var httpContext = _httpContextAccessor.HttpContext;
                var apiUrl = $"{_apiBaseUrl}/api/Review/customer/{customerId}";

                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

                // Authorization
                if (httpContext?.Request.Cookies.TryGetValue("access_token", out string token) == true)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                // Send the request
                var response = await client.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                var reviews = JsonSerializer.Deserialize<IEnumerable<Review>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return reviews ?? Enumerable.Empty<Review>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in CreateOrder: {ex.Message}");
                throw;
            }
        }

        public async Task<Review?> CreateReview(int productId, int Rating, string Text)
        {
            var client = _httpClientFactory.CreateClient();
            var httpContext = _httpContextAccessor.HttpContext;
            var apiUrl = $"{_apiBaseUrl}/api/Review/product/{productId}";

            var body = new ReviewDTO 
            {
                Text = Text,
                Rating = Rating
            };

            var jsonContent = JsonSerializer.Serialize(body);
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
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                var review = JsonSerializer.Deserialize<Review>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return new Review
                {
                    Id = review.Id,
                    Rating = review.Rating,
                    Text = review.Text,
                };
            }

            // Log error details if the response is not successful
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API Error: {response.StatusCode}, Content: {errorContent}");
            return null;
        }
    
    }
}