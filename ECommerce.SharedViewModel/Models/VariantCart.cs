namespace Ecommerce.SharedViewModel.Models
{
    public class VariantCart
    {
        public int VariantId { get; set; }
        public int CartId { get; set; }
        public Variant Variant { get; set; }
        public Cart Cart { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}