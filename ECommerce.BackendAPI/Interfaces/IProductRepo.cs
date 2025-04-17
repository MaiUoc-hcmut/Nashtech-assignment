using Ecommerce.SharedViewModel.Models;



namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product?> GetProductById(int id);
        // Task<bool> CreateProduct(Product product);
        // Task<ProductDTO> UpdateProduct(ProductDTO productDto);
        // Task<bool> DeleteProduct(int id);
        // Task<IEnumerable<ProductDTO>> GetProductsByCategory(int categoryId);
        // Task<IEnumerable<ProductDTO>> SearchProducts(string searchTerm);
        Task<bool> Save();
    }
}