using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.ParametersType;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ecommerce.BackendAPI.FiltersAction;


namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartController(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCartOfCustomer(int customerId)
        {
            var cart = await _cartRepository.GetCartOfCustomer(customerId);
            if (cart == null)
            {
                return NotFound(new { message = "Cart not found" });
            }

            var response = new
            {
                CartId = cart.Id,
                CustomerId = cart.Customer.Id,
                Variants = cart.VariantCarts.Select(vc => new
                {
                    vc.Variant.Id,
                    vc.Variant.SKU,
                    vc.Variant.Price,
                    vc.Variant.ImageUrl
                })
            };
            return Ok(response);
        }
    
        [HttpPost("{cartId}/{variantId}")]
        [ServiceFilter(typeof(AddToCartFilter))]
        public async Task<IActionResult> AddToCart(int cartId, int variantId)
        {
            var cart = HttpContext.Items["Cart"] as Cart;
            var variant = HttpContext.Items["Variant"] as Variant;

            if (cart == null || variant == null)
            {
                return BadRequest(new { message = "Invalid cart or variant ID" });
            }

            var result = await _cartRepository.AddToCart(cart, variant);
            if (!result)
            {
                return Conflict("Item already exists in the cart");
            }

            return Ok(new { message = "Item added to cart successfully" });
        }    
    
        [HttpDelete("{cartId}/{variantId}")]
        [ServiceFilter(typeof(RemoveFromCartFilter))]
        public async Task<IActionResult> RemoveFromCart(int cartId, int variantId)
        {
            var cart = HttpContext.Items["Cart"] as Cart;
            var variant = HttpContext.Items["Variant"] as Variant;

            if (cart == null || variant == null)
            {
                return BadRequest(new { message = "Invalid cart or variant ID" });
            }

            var result = await _cartRepository.RemoveFromCart(cart, variant);
            if (!result)
            {
                return NotFound("Item not found in the cart");
            }

            return Ok(new { message = "Item removed from cart successfully" });
        }
    }
}