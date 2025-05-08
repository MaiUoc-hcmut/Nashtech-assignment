namespace Ecommerce.SharedViewModel.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}