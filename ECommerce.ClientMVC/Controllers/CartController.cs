using Ecommerce.ClientMVC.Interface;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.ClientMVC.Filters;
using Newtonsoft.Json;

public class AddToCartParameter
{
    [JsonProperty("cartId")]
    public int cartId { get; set; }

    [JsonProperty("variantId")]
    public int variantId { get; set; }

    [JsonProperty("quantity")]
    public int quantity { get; set; } = 1;
}


namespace Ecommerce.ClientMVC.Controllers
{
    // [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _logger = logger;
            _logger.LogInformation("CartController initialized.");
            _cartService = cartService;
        }

        [HttpGet]
        [ServiceFilter(typeof(CustomerFilter))]
        public async Task<IActionResult> Index(int customerId)
        {
            var cartItems = await _cartService.GetCartOfCustomerAsync(customerId);
            return View(cartItems);
        }

        [HttpPost]
        [ServiceFilter(typeof(CustomerFilter))]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartParameter model)
        {
            var result = await _cartService.AddToCartAsync(model.cartId, model.variantId, model.quantity);
            
            return Json(new
            {
                IsSuccess = result,
                Message = result ? "Product added to the cart successfully." : "Failed to add the product to the cart."
            });
        }
    
        [HttpPost]
        [ServiceFilter(typeof(CustomerFilter))]
        public async Task<IActionResult> DeleteFromCart(int customerId, int cartId, int variantId)
        {
            var result = await _cartService.DeleteFromCartAsync(cartId, variantId);

            if (result)
            {
                TempData["SuccessMessage"] = "Item removed from the cart successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to remove the item from the cart.";
            }
            
            var cartItems = await _cartService.GetCartOfCustomerAsync(customerId);
            return View("Index", cartItems);
        }
    }
}