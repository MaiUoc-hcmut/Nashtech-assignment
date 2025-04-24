namespace Ecommerce.SharedViewModel.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public required string Text { get; set; }
        public Product Product { get; set; }
        public Customer Customer { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}