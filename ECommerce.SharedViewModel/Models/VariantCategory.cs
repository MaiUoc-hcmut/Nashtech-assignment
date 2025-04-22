namespace Ecommerce.SharedViewModel.Models
{
    public class VariantCategory
    {
        public int VariantId { get; set; }
        public int CategoryId { get; set; }
        public Variant Variant { get; set; }
        public Category Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}