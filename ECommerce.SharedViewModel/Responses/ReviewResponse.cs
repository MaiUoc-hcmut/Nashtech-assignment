namespace Ecommerce.SharedViewModel.Responses
{
    public class GetReviewsByProductResponse
    {
        public IEnumerable<object> Reviews { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public Dictionary<int, int> RatingsCount { get; set; }
    }

    public class GetReviewsResponse
    {
        public IEnumerable<object> Reviews { get; set; }
        public int TotalReviews { get; set; }
    }
}