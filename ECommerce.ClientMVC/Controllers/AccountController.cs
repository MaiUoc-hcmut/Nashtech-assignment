using Ecommerce.ClientMVC.Interface;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.SharedViewModel.ParametersType;
using System.Threading.Tasks;

namespace Ecommerce.ClientMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (await _authService.IsAuthenticated())
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginParameter model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.LoginAsync(model);
            
            if (result.Success)
            {
                _authService.StoreUserData(result);
                return RedirectToAction("Index", "Home");
            }
            
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (await _authService.IsAuthenticated())
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterParameter model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.RegisterAsync(model);
            
            if (result.Success)
            {
                return RedirectToAction("Login", "Account");
            }
            
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var result = await _authService.Logout();

            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, error = "Logout failed. Please try again." });
            }
        }
    }
}
