using System.Threading.Tasks;
using Ecommerce.ClientMVC.Attributes;
using Ecommerce.ClientMVC.Interface;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.ClientMVC.Controllers
{
    [Authorize]  // This controller requires authentication
    public class ProfileController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IOrderService _orderService;
        private readonly IReviewService _reviewService;

        public ProfileController(IAuthService authService, IOrderService orderService, IReviewService reviewService)
        {
            _authService = authService;
            _orderService = orderService;
            _reviewService = reviewService;
        }

        public IActionResult Index()
        {
            var model = new ProfileViewModel
            {
                Customer = _authService.GetCurrentUser(),
                Orders = new List<Order>(),
                Reviews = new List<Review>()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult GetAccountInfoPartial()
        {
            var Customer = _authService.GetCurrentUser();
            return PartialView("_AccountInfoPartial", Customer);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersPartial(int customerId)
        {
            var orders = await _orderService.GetOrdersOfCustomer(customerId);
            return PartialView("_OrdersPartial", orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetReviewsPartial(int customerId)
        {
            var reviews = await _reviewService.GetReviewsOfCustomer(customerId);
            return PartialView("_ReviewsPartial", reviews);
        }
    }
}