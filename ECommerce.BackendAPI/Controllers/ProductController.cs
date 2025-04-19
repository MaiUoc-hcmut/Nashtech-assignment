using Microsoft.AspNetCore.Mvc;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.ParametersType;
using AutoMapper;
using System.Text.Json;
using Microsoft.AspNetCore.Http;


namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IVariantRepository _variantRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ProductController
        (
            IProductRepository productRepository, 
            IVariantRepository variantRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper
        )
        {
            _productRepository = productRepository;
            _variantRepository = variantRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productRepository.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id, [FromQuery] string? includeVariant = null)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            if (includeVariant != null && includeVariant.ToLower() == "true")
            {
                var variants = await _variantRepository.GetVariantsByProductId(id);
                product.Variants = variants.ToList();
            }

            return Ok(product);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateProduct
        (
            [FromForm] ProductDTO productDto, 
            [FromForm] IEnumerable<IFormFile> images,
            [FromForm] string? variants = null
        )
        {
            var transaction = await _productRepository.BeginTransactionAsync();
            try
            {
                if (productDto == null)
                {
                    return BadRequest("Product data is null.");
                }

                var urls = HttpContext.Items["urls"] as Dictionary<string, string>;
                if (urls != null && urls.TryGetValue("product", out var productUrl))
                {
                    productDto.ImageUrl = productUrl;
                }
                else
                {
                    productDto.ImageUrl = "";
                }
                IList<CreateVariantsOfProductParameter> variantList = new List<CreateVariantsOfProductParameter>();
                if (!string.IsNullOrEmpty(variants))
                {
                    try
                    {
                        variantList = JsonSerializer.Deserialize<List<CreateVariantsOfProductParameter>>(variants) ?? new List<CreateVariantsOfProductParameter>();
                    }
                    catch (JsonException ex)
                    {
                        return BadRequest($"Invalid JSON format for variants: {ex.Message}");
                    }  
                }                

                var product = _mapper.Map<Product>(productDto);
                var productCreated = await _productRepository.CreateProduct(product);

                if (variantList.Count > 0)
                {
                    foreach (var variant in variantList)
                    {
                        if (urls != null && urls.TryGetValue(variant.Key, out var variantUrl))
                        {
                            variant.ImageUrl = variantUrl;
                        }

                        var variantCategories = new List<VariantCategory>();
                        foreach (var categoryId in variant.Categories)
                        {
                            // var category = await _categoryRepository.GetCategoryById(categoryId);
                            // if (category == null)
                            // {
                            //     throw new Exception($"Category with ID {categoryId} does not exist.");
                            // }
                            variantCategories.Add(new VariantCategory { CategoryId = categoryId });
                        }
                        var variantEntity = _mapper.Map<Variant>(variant);
                        variantEntity.Product = productCreated;
                        variantEntity.VariantCategories = variantCategories;
                        await _variantRepository.CreateVariant(variantEntity);
                    }
                }

                await transaction.CommitAsync();

                return Ok("Product created successfully.");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDTO productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Product data is null.");
            }

            var product = _mapper.Map<Product>(productDto);
            product.Id = id;

            if (!await _productRepository.UpdateProduct(product))
            {
                return NotFound($"Product with ID {id} not found.");
            }

            return Ok($"Product {productDto.Name} updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!await _productRepository.DeleteProduct(id))
            {
                return NotFound($"Product with ID {id} not found.");
            }

            return Ok($"Product with ID {id} deleted successfully.");
        }
    }
}