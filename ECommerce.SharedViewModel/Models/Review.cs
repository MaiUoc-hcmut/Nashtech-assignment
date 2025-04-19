namespace Ecommerce.SharedViewModel.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public required string Text { get; set; }
        public required Product Product { get; set; }
        public required Customer Customer { get; set; }
    }
}