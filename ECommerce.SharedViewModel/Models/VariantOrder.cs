namespace Ecommerce.SharedViewModel.Models
{
    public class VariantOrder
    {        
        public int VariantId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public Variant Variant { get; set; }
        public Order Order { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}