namespace Ecommerce.SharedViewModel.Models
{
    public class ProductCart
    {
        public int ProductId { get; set; }
        public int CartId { get; set; }
        public Product Product { get; set; }
        public Cart Cart { get; set; }
    }
}