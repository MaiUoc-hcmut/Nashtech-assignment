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
        public required string Description { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateVariantsOfProductParameter
    {
        public required string SKU { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public required string Key { get; set; }
        public required IEnumerable<int> Categories { get; set; }
    }
}