using Ecommerce.SharedViewModel.Models;

namespace Ecommerce.SharedViewModel.ParametersType
{
    public class CreateOrderParameter
    {
        public required int Amount { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public required ICollection<int> Variants { get; set; }
    }
}