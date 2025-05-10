using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.ParametersType;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.BackendAPI.FiltersAction;


namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCartOfCustomer(int customerId)
        {
            var cart = await _cartRepository.GetCartOfCustomerAsync(customerId, true);
            if (cart == null)
            {
                return NotFound(new { message = "Cart not found" });
            }

            return Ok(new
            {
                CartId = cart.Id,
                cart.Customer,
                Variants = cart.VariantCarts.Select(vc => new
                {
                    Product = new
                    {
                        vc.Variant.Product.Id,
                        vc.Variant.Product.Name,
                        vc.Variant.Product.Description,
                        vc.Variant.Product.ImageUrl,
                    },
                    vc.Variant.Id,
                    vc.Quantity,
                    vc.Variant.Price,
                    vc.Variant.ImageUrl,
                    vc.Variant.SKU,
                    Color = vc.Variant.VariantCategories
                        .Where(vc => vc.Category.ParentCategory != null && vc.Category.ParentCategory.Name.ToLower().Contains("color"))
                        .Select(vc => new
                        {
                            vc.Category.Id,
                            vc.Category.Name
                        })
                        .FirstOrDefault(),
                    Size = vc.Variant.VariantCategories
                        .Where(vc => vc.Category.ParentCategory != null && vc.Category.ParentCategory.Name.ToLower().Contains("size"))
                        .Select(vc => new
                        {
                            vc.Category.Id,
                            vc.Category.Name
                        })
                        .FirstOrDefault()
                })
            });
        }

        [HttpPost("{cartId}/{variantId}")]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(AddToCartFilter))]
        public async Task<IActionResult> AddToCart(int cartId, int variantId, [FromBody] int quantity = 1)
        {
            Console.WriteLine("AddToCart filter executed.");
            var cart = HttpContext.Items["Cart"] as Cart;
            var variant = HttpContext.Items["Variant"] as Variant;

            if (cart == null || variant == null)
            {
                return BadRequest(new { message = "Invalid cart or variant ID" });
            }

            var result = await _cartRepository.AddToCartAsync(cart, variant, quantity);
            if (!result)
            {
                return Conflict("Item already exists in the cart");
            }

            return Ok(new { message = "Item added to cart successfully" });
        }    
    
        [HttpDelete("{cartId}/{variantId}")]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(RemoveFromCartFilter))]
        public async Task<IActionResult> RemoveFromCart(int cartId, int variantId)
        {
            var cart = HttpContext.Items["Cart"] as Cart;
            var variant = HttpContext.Items["Variant"] as Variant;

            if (cart == null || variant == null)
            {
                return BadRequest(new { message = "Invalid cart or variant ID" });
            }

            var result = await _cartRepository.RemoveFromCartAsync(cart, variant);
            if (!result)
            {
                return NotFound("Item not found in the cart");
            }

            return Ok(new { message = "Item removed from cart successfully" });
        }
    }
}