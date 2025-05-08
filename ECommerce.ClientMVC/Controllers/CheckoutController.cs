using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.ClientMVC.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}