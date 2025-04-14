namespace Ecommerce.SharedViewModel.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public ICollection<ProductCart> ProductCarts { get; set; }
        public Customer Customer { get; set; }
    }
}