namespace Ecommerce.SharedViewModel.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public ICollection<VariantCart> VariantCarts { get; set; }
        public Customer Customer { get; set; }
    }
}