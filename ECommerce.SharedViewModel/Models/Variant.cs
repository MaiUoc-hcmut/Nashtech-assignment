namespace Ecommerce.SharedViewModel.Models
{
    public class Variant
    {
        public int Id { get; set; }
        public required string SKU { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public Product Product { get; set; }
        public ICollection<VariantCategory> VariantCategories { get; set; }
        public ICollection<VariantOrder> VariantOrders { get; set; }
        public ICollection<VariantCart> VariantCarts { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}