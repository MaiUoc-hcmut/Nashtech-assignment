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

        public async Task<List<GetAllProductsResponse>> GetAllProductsAsync
        (
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "UpdatedAt",
            bool isAsc = true,
            int? classificationId = null,
            int minPrice = 0,
            int maxPrice = 999999999,
            string? search = null
        )
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync
            (
                $"{_apiBaseUrl}/api/Product?pageNumber={pageNumber}&pageSize={pageSize}&sortBy={sortBy}&isAsc={isAsc}&classificationId={classificationId}&minPrice={minPrice}&maxPrice={maxPrice}&search={search}"
            );
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<GetAllProductsResponse>>() ?? new List<GetAllProductsResponse>();
            }
            return new List<GetAllProductsResponse>();
        }

        public async Task<ProductDetail?> GetProductByIdAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Product/{id}?includeVariant=true");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProductDetail>() ?? null;
            }
            return null;
        }
    }
}