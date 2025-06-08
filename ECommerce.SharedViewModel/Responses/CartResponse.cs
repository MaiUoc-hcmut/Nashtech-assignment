using Ecommerce.SharedViewModel.DTOs;

namespace Ecommerce.SharedViewModel.Responses
{
    public class ProductOfCartResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public required string Description { get; set; }
        public int Price { get; set; }
    }
    public class VariantOfCartResponse
    {
        public int Id { get; set; }
        public required string SKU { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required ProductOfCartResponse Product { get; set; }
        public required CategoryDTO Color { get; set; }
        public required CategoryDTO Size { get; set; }
    }

    public class CartItemResponse
    {
        public int CartId { get; set; }
        public CustomerDTO? Customer { get; set; }
        public IEnumerable<VariantOfCartResponse> Variants { get; set; }
    }
}