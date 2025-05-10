using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.BackendAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariantController : ControllerBase
    {
        private readonly IVariantRepository _variantRepository;
        private readonly IProductRepository _productRepository;

        public VariantController(IVariantRepository variantRepository, IProductRepository productRepository)
        {
            _variantRepository = variantRepository;
            _productRepository = productRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Variant>> GetVariantById(int id)
        {
            var variant = await _variantRepository.GetVariantByIdAsync(id);
            if (variant == null) return NotFound();
            return Ok(variant);
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<Variant>>> GetVariantsByProductId(int productId)
        {
            var response = await _variantRepository.GetVariantsByProductIdAsync(productId);
            var variants = response.Select(v => new Variant {
                Id = v.Id,
                SKU = v.SKU,
                Price = v.Price,
                StockQuantity = v.StockQuantity,
                ImageUrl = v.ImageUrl,
                CreatedAt = v.CreatedAt,
                UpdatedAt = v.UpdatedAt,
            }).ToList();
            if (variants == null || !variants.Any()) return NotFound();
            return Ok(variants);
        }

        [HttpPost]
        public async Task<ActionResult> CreateVariant
        (
            [FromForm] VariantDTO variantDto, 
            // [FromForm] IFormFile? image,
            [FromForm] string Categories,
            [FromForm] int productId
        )
        {
            if (variantDto == null) return BadRequest("Invalid variant data.");
            if (Categories == null) return BadRequest("Invalid category data.");

            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null) return BadRequest(new { Error = "Invalid productId" });

            var variant = new Variant {
                SKU = variantDto.SKU,
                StockQuantity = variantDto.StockQuantity,
                Price = variantDto.Price,
                Product = product
            };
            var url = HttpContext.Items["Url"] as string;

            var categories = JsonSerializer.Deserialize<List<VariantCategory>>(Categories);
            var categoryList = new List<VariantCategory>();
            if (categories == null || categories.Count == 0)
            {
                return BadRequest("Invalid category data.");
            }
            foreach (var category in categories)
            {
                categoryList.Add(new VariantCategory
                {
                    CategoryId = category.CategoryId
                });
            }

            variant.ImageUrl = url != null ? url : "";
            variant.VariantCategories = categoryList;
            await _variantRepository.CreateVariantAsync(variant);
            return Ok("Variant created successfully.");
        }
    
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVariant
        (
            int id, 
            [FromForm] VariantDTO variantDto
            // [FromForm] IFormFile? image
        )
        {
            if (variantDto == null) return BadRequest("Invalid variant data.");

            var variant = await _variantRepository.GetVariantByIdAsync(id);
            if (variant == null) return NotFound("Variant not found.");

            var url = HttpContext.Items["Url"] as string;
            variant.ImageUrl = url != null ? url : variant.ImageUrl;
            foreach (var property in typeof(VariantDTO).GetProperties())
            {
                var variantProperty = typeof(Variant).GetProperty(property.Name);
                if (variantProperty != null && variantProperty.CanWrite)
                {
                    var value = property.GetValue(variantDto);
                    variantProperty.SetValue(variant, value);
                }
            }

            await _variantRepository.UpdateVariantAsync(variant);
            return Ok("Variant updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVariant(int id)
        {
            var variant = await _variantRepository.GetVariantByIdAsync(id);
            if (variant == null) return NotFound("Variant not found.");

            await _variantRepository.DeleteVariantAsync(id);
            return Ok("Variant deleted successfully.");
        }
    }
}