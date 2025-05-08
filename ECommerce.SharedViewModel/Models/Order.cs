namespace Ecommerce.SharedViewModel.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Status { get; set; } = "Success";
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Customer Customer { get; set; }
        public ICollection<VariantOrder> VariantOrders { get; set; }
    }
}