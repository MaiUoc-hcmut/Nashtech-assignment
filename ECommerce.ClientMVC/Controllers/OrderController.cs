using Ecommerce.ClientMVC.Interface;
using Ecommerce.SharedViewModel.Responses;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.SharedViewModel.ParametersType;
using System.Text.Json;

namespace Ecommerce.ClientMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> CreateOrder
        (
            string customerName,
            string customerEmail,
            string customerPhoneNumber,
            string customerAddress,
            // int customerId,
            string variantList,
            int totalAmount 
        )
        {
            var variantListDeserialize = JsonSerializer.Deserialize<IList<VariantInCreateOrder>>(variantList);
            foreach (var variant in variantListDeserialize)
            {
                if (variant.Quantity <= 0)
                {
                    return BadRequest("Quantity must be greater than zero for each variant.");
                }
            }
            var result = await _orderService.CreateOrderAsync
                (
                    customerName, 
                    customerEmail, 
                    customerPhoneNumber, 
                    customerAddress, 
                    // customerId, 
                    variantListDeserialize, 
                    totalAmount
                );

            return View("OrderSuccess", result);
        }
    }
}