using System.ComponentModel.DataAnnotations;

namespace Ecommerce.SharedViewModel.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Price { get; set; }
        public required string Description { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Variant> Variants { get; set; }
        public ICollection<ProductClassification> ProductClassifications { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}