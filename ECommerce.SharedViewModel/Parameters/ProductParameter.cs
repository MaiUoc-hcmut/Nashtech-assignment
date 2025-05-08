namespace Ecommerce.SharedViewModel.ParametersType
{
    public class GetProductParameter
    {
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
    }

    public class CreateProductParameter
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int Price { get; set; }
    }

    public class CreateVariantsOfProductParameter
    {
        public required string SKU { get; set; }
        public string Price { get; set; } = "0";
        public string StockQuantity { get; set; } = "0";
        public string ImageUrl { get; set; } = string.Empty;
        public required string Key { get; set; }
        public required IEnumerable<int> Categories { get; set; }
    }
}