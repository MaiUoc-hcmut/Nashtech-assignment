namespace Ecommerce.SharedViewModel.DTOs
{
    public class VariantDTO
    {
        public int Id { get; set; }
        public required string SKU { get; set; }
        public int Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}