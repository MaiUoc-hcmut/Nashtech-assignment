namespace Ecommerce.SharedViewModel.Responses
{
    public class ProductInGetOrdersOfCustomer
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ImageUrl { get; set; } = string.Empty;
        public int Price { get; set; }
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

    public class CustomerInOrderResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class ProductInGetOrderById
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string ImageUrl { get; set; } = string.Empty;
        public int Price { get; set; }
    }

    public class VariantInGetOrderById
    {
        public int Id { get; set; }
        public required string SKU { get; set; }
        public int Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public required string Color { get; set; }
        public required string Size { get; set; }
        public ProductInGetOrderById Product { get; set; }
    }

    public class GetOrderByIdResponse
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Status { get; set; } = "Success";
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public CustomerInOrderResponse Customer { get; set; }
        public IEnumerable<VariantInGetOrderById> Variants { get; set; }
    }
}