using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.DTOs;
using Microsoft.EntityFrameworkCore.Storage;



namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IProductRepository
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product?> GetProductById(int id);
        Task<Product> CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int id);
        // Task<IEnumerable<ProductDTO>> GetProductsByCategory(int categoryId);
        // Task<IEnumerable<ProductDTO>> SearchProducts(string searchTerm);
        Task<bool> Save();
    }
}