using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.Responses;

namespace Ecommerce.ClientMVC.Interface
{
    public interface IProductService
    {
        Task<GetAllProductsResponse> GetAllProductsAsync
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
        );
        Task<ProductDetail?> GetProductByIdAsync(int id);
    }
}