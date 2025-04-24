using Ecommerce.SharedViewModel.Models;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.BackendAPI.FiltersAction;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.SharedViewModel.ParametersType;


namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet()]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(GetAllOrdersFilter))]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders();
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(GetOrderFilter))]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            if (order == null) return NotFound();

            return Ok(order);
        }

        [HttpGet("customer/{customerId}")]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(GetOrderFilter))]
        public async Task<IActionResult> GetOrdersByUserId(int customerId)
        {
            var orders = await _orderRepository.GetOrderByUserId(customerId);
            if (orders == null) return NotFound();

            return Ok(orders);
        }

        [HttpGet("product/{productId}")]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(GetOrderFilter))]
        public async Task<IActionResult> GetOrdersOfProduct(int productId)
        {
            var orders = await _orderRepository.GetOrdersOfProduct(productId);

            return Ok(orders);
        }

        [HttpPost]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(CreateOrderFilter))]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderParameter request)
        {
            if (request == null) return BadRequest("Order cannot be null");

            var customer = HttpContext.Items["Customer"] as Customer;
            var variants = HttpContext.Items["Variants"] as List<Variant>;

            if (customer == null || variants == null || variants.Count == 0) {
                return BadRequest(new { Error = "Invalid request" });
            }

            var order = new Order {
                Customer = customer,
                Amount = request.Amount,
                VariantOrders = variants.Select(v => new VariantOrder {
                    Variant = v
                }).ToList()
            };
            var result = await _orderRepository.CreateOrder(order);

            if (!result) return StatusCode(500, "Something went wrong while creating the order");

            return CreatedAtAction(nameof(GetOrderById), new { orderId = order.Id }, order);
        }
    }
}