using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Responses;
using Microsoft.EntityFrameworkCore.Storage;



namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IProductRepository
    {
        public void AttachProductClassification(ProductClassification productClassification);
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<(int TotalProducts, IEnumerable<ProductsGetAllProductsResponse> Products)> GetAllProductsAsync(
            int pageNumber,
            int pageSize,
            string sortBy,
            bool isAsc,
            int? classificationId,
            int minPrice,
            int maxPrice,
            string? search
        );
        Task<Product?> GetProductByIdAsync(int id, bool includeRelated = true);
        Task<Product> CreateProductAsync(Product product, IList<Classification> classificationList);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
        // Task<IEnumerable<ProductDTO>> GetProductsByCategory(int categoryId);
        // Task<IEnumerable<ProductDTO>> SearchProducts(string searchTerm);
        Task<bool> SaveAsync();
    }
}