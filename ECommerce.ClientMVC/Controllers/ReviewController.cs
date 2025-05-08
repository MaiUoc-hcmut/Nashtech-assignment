using Ecommerce.ClientMVC.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class CreateReviewParameter
{
    [JsonProperty("productId")]
    public int productId { get; set; }

    [JsonProperty("rating")]
    public int rating { get; set; }

    [JsonProperty("text")]
    public string text { get; set; }
}


namespace Ecommerce.ClientMVC.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // [HttpGet]
        // public async Task<IActionResult> GetReviewsOfCustomer(int customerId)
        // {
        //     _reviewService.
        // }

        
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewParameter model)
        {  
            var result = await _reviewService.CreateReview(model.productId, model.rating, model.text);

            return Json(new { success = true, redirectUrl = Url.Action("Index", "Profile") });
        }
    }
}