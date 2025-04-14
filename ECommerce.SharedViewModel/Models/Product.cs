namespace Ecommerce.SharedViewModel.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int StockQuantity { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }
        public ICollection<ProductOrder> ProductOrders { get; set; }
        public ICollection<ProductCart> ProductCarts { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}