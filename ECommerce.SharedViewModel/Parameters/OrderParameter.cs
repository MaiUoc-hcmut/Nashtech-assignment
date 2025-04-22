namespace Ecommerce.SharedViewModel.ParametersType
{
    public class CreateOrderParameter
    {
        public required decimal Amount { get; set; }
        public required ICollection<int> Variants { get; set; }
    }
}