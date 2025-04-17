namespace Ecommerce.SharedViewModel.DTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public required string Text { get; set; }
        public int Rating { get; set; }
    }
}