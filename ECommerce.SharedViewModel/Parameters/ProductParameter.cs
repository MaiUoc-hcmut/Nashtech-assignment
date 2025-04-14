namespace Ecommerce.SharedViewModel.ParametersType
{
    public class GetProductParameter
    {
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}