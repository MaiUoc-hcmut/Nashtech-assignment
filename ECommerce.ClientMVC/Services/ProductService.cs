using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.ClientMVC.Interface;
using Ecommerce.SharedViewModel.Responses;

namespace Ecommerce.ClientMVC.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _apiBaseUrl;

        public ProductService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _apiBaseUrl = "http://localhost:5113";
        }

        public async Task<GetAllProductsResponse> GetAllProductsAsync
        (
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "UpdatedAt",
            bool isAsc = true,
            int? classificationId = null,
            int minPrice = 0,
            int maxPrice = 999999999,
            double minRating = 0,
            double maxRating = 5,
            string startDate = "01/01/2025",
            string? endDate = null,
            string? search = null
        )
        {
            
            Console.WriteLine("==================================================");
            endDate ??= DateTime.Now.ToString("yyyy-MM-dd");
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync
            (
                $"{_apiBaseUrl}/api/Product?pageNumber={pageNumber}&pageSize={pageSize}&sortBy={sortBy}&isAsc={isAsc}&classificationId={classificationId}&minPrice={minPrice}&maxPrice={maxPrice}&search={search}&startDate={startDate}&endDate={endDate}"
            );
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<GetAllProductsResponse>() ?? new GetAllProductsResponse();
            }
            return new GetAllProductsResponse();
        }

        public async Task<ProductDetail?> GetProductByIdAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Product/{id}?includeVariant=true");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProductDetail>() ?? null;
            }
            return null;
        }
    }
}