namespace Ecommerce.SharedViewModel.Models
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public Customer Customer { get; set; }
        public ICollection<VariantOrder> VariantOrders { get; set; }
    }
}