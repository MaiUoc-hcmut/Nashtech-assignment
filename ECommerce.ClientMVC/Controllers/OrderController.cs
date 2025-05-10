using Ecommerce.ClientMVC.Interface;
using Ecommerce.SharedViewModel.Responses;
using Microsoft.AspNetCore.Mvc;

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
            string variantIdList,
            int totalAmount 
        )
        {
            var result = await _orderService.CreateOrderAsync
                (
                    customerName, 
                    customerEmail, 
                    customerPhoneNumber, 
                    customerAddress, 
                    // customerId, 
                    variantIdList, 
                    totalAmount
                );

            Console.WriteLine(result.TotalAmount);
            return View("OrderSuccess", result);
        }
    }
}