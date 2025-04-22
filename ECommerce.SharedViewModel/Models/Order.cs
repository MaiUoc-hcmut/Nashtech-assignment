namespace Ecommerce.SharedViewModel.Models
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string status { get; set; } = "Success";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Customer Customer { get; set; }
        public ICollection<VariantOrder> VariantOrders { get; set; }
    }
}