namespace Ecommerce.SharedViewModel.Responses
{
    public class ProductInGetOrdersOfCustomer
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ImageUrl { get; set; } = string.Empty;
    }

    public class VariantInGetOrdersOfCustomer
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public required string Color { get; set; }
        public required string Size { get; set; }
        public required ProductInGetOrdersOfCustomer Product { get; set; }
    }

    public class CreateOrderResponse
    {
        public int Id { get; set; }
        public int TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GetOrdersOfCustomerResponse
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Amount { get; set; }
        public IEnumerable<VariantInGetOrdersOfCustomer> Variants { get; set; }
    }
}