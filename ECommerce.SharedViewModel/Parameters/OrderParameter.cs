using Ecommerce.SharedViewModel.Models;

namespace Ecommerce.SharedViewModel.ParametersType
{
    public class VariantInCreateOrder
    {
        public required int Id { get; set; }
        public required int Quantity { get; set; }
    }


    public class CreateOrderParameter
    {
        public required int Amount { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public required ICollection<VariantInCreateOrder> Variants { get; set; }
    }
}