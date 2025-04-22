namespace Ecommerce.SharedViewModel.Models
{
    public class ProductClassification
    {
        public int ProductId { get; set; }
        public int ClassificationId { get; set; }
        public Product Product { get; set; }
        public Classification Classification { get; set; }
    }
}