using Microsoft.AspNetCore.Mvc;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.ParametersType;
using System.Text.Json;


namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IVariantRepository _variantRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IClassificationRepository _classificationRepository;

        public ProductController
        (
            IProductRepository productRepository, 
            IVariantRepository variantRepository,
            ICategoryRepository categoryRepository,
            IClassificationRepository classificationRepository
        )
        {
            _productRepository = productRepository;
            _variantRepository = variantRepository;
            _categoryRepository = categoryRepository;
            _classificationRepository = classificationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts
        (
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "UpdatedAt",
            [FromQuery] bool isAsc = true,
            [FromQuery] int? classificationId = null,
            [FromQuery] decimal minPrice = 0,
            [FromQuery] decimal maxPrice = 999999999
        )
        {
            var products = await _productRepository.GetAllProducts
                (
                    pageNumber, 
                    pageSize, 
                    sortBy, 
                    isAsc,
                    classificationId,
                    minPrice,
                    maxPrice
                );
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
            
            List<object>? variants = null;
            if (includeVariant != null && includeVariant.ToLower() == "true")
            {
                var variantEntities = await _variantRepository.GetVariantsByProductId(id);
                variants = variantEntities.Select(v => new
                {
                    v.Id,
                    v.SKU,
                    v.Price,
                    v.StockQuantity,
                    v.ImageUrl,
                    v.CreatedAt,
                    v.UpdatedAt
                }).ToList<object>();
            }
            
            return Ok(new {
                product.Id,
                product.Name,
                product.Price,
                product.Description,
                product.ImageUrl,
                product.CreatedAt,
                product.UpdatedAt,
                AverageRating = product.Reviews != null && product.Reviews.Any() 
                    ? product.Reviews.Average(r => r.Rating) 
                    : 0,
                TotalOrders = product.Variants
                        .SelectMany(v => v.VariantOrders)
                        .Count(),
                Variants = variants
            });
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateProduct
        (
            [FromForm] ProductDTO productDto, 
            [FromForm] string classifications,
            [FromForm] string? variants = null
        )
        {
            var transaction = await _productRepository.BeginTransactionAsync();
            try
            {
                if (productDto == null || classifications == null)
                {
                    return BadRequest("Product data or classification is null.");
                }

                var urls = HttpContext.Items["UploadedUrls"] as Dictionary<string, string>;
                if (urls != null && urls.TryGetValue("product", out var productUrl))
                {
                    productDto.ImageUrl = productUrl;
                }
                else
                {
                    productDto.ImageUrl = "";
                }

                var classificationIdList = JsonSerializer.Deserialize<List<int>>(classifications);
                if (classificationIdList == null || classificationIdList.Count == 0) return BadRequest(new { Error = "Classification is required" });

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
                var classificationList = new List<Classification>();
                foreach (int Id in classificationIdList) {
                    var classification = await _classificationRepository.GetClassificationById(Id);
                    if (classification == null) return BadRequest(new { Error = $"Classification with Id = {Id} not found"});
                    classificationList.Add(classification);
                }

                var product = new Product {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    Description = productDto.Description
                };
                var productCreated = await _productRepository.CreateProduct(product, classificationList);

                if (variantList.Count > 0)
                {
                    foreach (var variantDto in variantList)
                    {
                        if (urls != null && urls.TryGetValue(variantDto.Key, out var variantUrl))
                        {
                            variantDto.ImageUrl = variantUrl;
                        }

                        var variantCategories = new List<VariantCategory>();
                        foreach (var categoryId in variantDto.Categories)
                        {
                            variantCategories.Add(new VariantCategory { CategoryId = categoryId });
                        }
                        var variantEntity = new Variant {
                            SKU = variantDto.SKU,
                            Price = variantDto.Price,
                            StockQuantity = variantDto.StockQuantity,
                            ImageUrl = variantDto.ImageUrl
                        };
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

            var product = new Product {
                Name = productDto.Name,
                Price = productDto.Price,
                Description = productDto.Description,
                Id = id
            };

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