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
        Task<IEnumerable<GetAllProductsResponse>> GetAllProducts(
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "Price",
            bool isAsc = true,
            int? classificationId = null
        );
        Task<Product?> GetProductById(int id);
        Task<Product> CreateProduct(Product product, IList<Classification> classificationList);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int id);
        // Task<IEnumerable<ProductDTO>> GetProductsByCategory(int categoryId);
        // Task<IEnumerable<ProductDTO>> SearchProducts(string searchTerm);
        Task<bool> Save();
    }
}