using System.Threading.Tasks;
using Ecommerce.ClientMVC.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.ClientMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _logger = logger;
            _logger.LogInformation("ProductController initialized.");
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index
        (
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "UpdatedAt",
            bool isAsc = true,
            int? classificationId = null,
            int minPrice = 0,
            int maxPrice = 999999999,
            string? search = null
        )
        {
            var products = await _productService.GetAllProductsAsync
            (
                pageNumber,
                pageSize,
                sortBy,
                isAsc,
                classificationId,
                minPrice,
                maxPrice,
                search
            );
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View("Detail", product);
        }
    }
}