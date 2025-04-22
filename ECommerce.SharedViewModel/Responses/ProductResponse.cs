using Ecommerce.SharedViewModel.Models;

namespace Ecommerce.SharedViewModel.Responses
{
    public class GetAllProductsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public double AverageRating { get; set; }
        public int TotalOrders { get; set; }
    }
}