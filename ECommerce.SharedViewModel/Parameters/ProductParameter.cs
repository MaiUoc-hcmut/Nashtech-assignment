namespace Ecommerce.SharedViewModel.ParametersType
{
    public class GetProductParameter
    {
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }

    public class CreateProductParameter
    {
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public int MaterialId { get; set; }
    }
}