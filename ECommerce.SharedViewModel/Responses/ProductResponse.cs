using Ecommerce.SharedViewModel.Models;

namespace Ecommerce.SharedViewModel.Responses
{
    public class CustomerOfReview
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
    }

    public class ReviewItemOfProductDetail
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public CustomerOfReview Customer { get; set; }
    }

    public class ReviewDetailsOfProduct
    {
        public int TotalReviews { get; set; }
        public int OneStar { get; set; }
        public int OneStarPercent { get; set; }
        public int TwoStar { get; set; }
        public int TwoStarPercent { get; set; }
        public int ThreeStar { get; set; }
        public int ThreeStarPercent { get; set; }
        public int FourStar { get; set; }
        public int FourStarPercent { get; set; }
        public int FiveStar { get; set; }
        public int FiveStarPercent { get; set; }
        public IEnumerable<ReviewItemOfProductDetail> Reviews {get; set; }
    }

    public class Sizes
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }

    public class Colors
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public IEnumerable<Sizes> Sizes { get; set; } = new List<Sizes>();
    }

    public class ProductsGetAllProductsResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public required string Description { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public int TotalOrders { get; set; }
        public IEnumerable<string> ClassificationNames { get; set; }
    }
    
    public class GetAllProductsResponse
    {
        public int TotalProducts { get; set; }
        public ProductsGetAllProductsResponse[] Products { get; set; }
    }

    public class ProductDetail
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Price { get; set; }
        public required string Description { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int TotalOrders { get; set; }
        public double AverageRating { get; set; }
        public IEnumerable<Colors> Colors { get; set; } = new List<Colors>();
        public required ReviewDetailsOfProduct ReviewDetails { get; set; }
        public required List<Classification> Classifications { get; set; }
        public List<VariantResponse> Variants { get; set; } = new List<VariantResponse>();
    }
}