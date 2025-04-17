namespace Ecommerce.SharedViewModel.Models
{
    public class VariantOrder
    {        
        public int VariantId { get; set; }
        public int OrderId { get; set; }
        public Variant Variant { get; set; }
        public Order Order { get; set; }
    }
}