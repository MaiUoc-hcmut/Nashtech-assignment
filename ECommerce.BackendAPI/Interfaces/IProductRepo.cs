using Ecommerce.SharedViewModel.Models;



namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductById(int id);
        // Task<IEnumerable<ProductDTO>> GetAllProducts();
        // Task<ProductDTO> AddProduct(ProductDTO productDto);
        // Task<ProductDTO> UpdateProduct(ProductDTO productDto);
        // Task<bool> DeleteProduct(int id);
        // Task<IEnumerable<ProductDTO>> GetProductsByCategory(int categoryId);
        // Task<IEnumerable<ProductDTO>> SearchProducts(string searchTerm);
    }
}