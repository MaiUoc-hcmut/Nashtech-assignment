using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.BackendAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Text.Json;


namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariantController : ControllerBase
    {
        private readonly IVariantRepository _variantRepository;
        private readonly IMapper _mapper;

        public VariantController(IVariantRepository variantRepository, IMapper mapper)
        {
            _mapper = mapper;
            _variantRepository = variantRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Variant>> GetVariantById(int id)
        {
            var variant = await _variantRepository.GetVariantById(id);
            if (variant == null) return NotFound();
            return Ok(variant);
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<Variant>>> GetVariantsByProductId(int productId)
        {
            var variants = _mapper.Map<IEnumerable<Variant>>(await _variantRepository.GetVariantsByProductId(productId));
            if (variants == null || !variants.Any()) return NotFound();
            return Ok(variants);
        }

        [HttpPost]
        public async Task<ActionResult> CreateVariant
        (
            [FromForm] VariantDTO variantDto, 
            // [FromForm] IFormFile? image,
            [FromForm] string Categories
        )
        {
            if (variantDto == null) return BadRequest("Invalid variant data.");
            if (Categories == null) return BadRequest("Invalid category data.");

            var variant = _mapper.Map<Variant>(variantDto);
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
            await _variantRepository.CreateVariant(variant);
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

            var variant = await _variantRepository.GetVariantById(id);
            if (variant == null) return NotFound("Variant not found.");

            var url = HttpContext.Items["Url"] as string;
            variant.ImageUrl = url != null ? url : variant.ImageUrl;

            _mapper.Map(variantDto, variant);
            await _variantRepository.UpdateVariant(variant);
            return Ok("Variant updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVariant(int id)
        {
            var variant = await _variantRepository.GetVariantById(id);
            if (variant == null) return NotFound("Variant not found.");

            await _variantRepository.DeleteVariant(id);
            return Ok("Variant deleted successfully.");
        }
    }
}