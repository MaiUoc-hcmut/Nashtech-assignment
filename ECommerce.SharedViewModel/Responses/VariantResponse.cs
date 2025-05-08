using Ecommerce.SharedViewModel.Models;

namespace Ecommerce.SharedViewModel.Responses
{
    public class VariantResponse
    {
        public int Id { get; set; }
        public required string SKU { get; set; }
        public int Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required Category Color { get; set; }
        public required Category Size { get; set; }
    }
}