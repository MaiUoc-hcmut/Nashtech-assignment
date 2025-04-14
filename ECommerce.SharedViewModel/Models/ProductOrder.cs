namespace Ecommerce.SharedViewModel.Models
{
    public class ProductOrder
    {        
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}