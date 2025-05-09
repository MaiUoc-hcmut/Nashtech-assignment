using Microsoft.AspNetCore.Mvc;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.ParametersType;
using Ecommerce.BackendAPI.FiltersAction;
using System.Text.Json;
using Ecommerce.BackendAPI.Services;
using Ecommerce.SharedViewModel.Responses;


namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IVariantRepository _variantRepository;
        private readonly IAuthService _authService;
        private readonly IClassificationRepository _classificationRepository;

        public ProductController
        (
            IProductRepository productRepository, 
            IVariantRepository variantRepository,
            IAuthService authService,
            IClassificationRepository classificationRepository
        )
        {
            _productRepository = productRepository;
            _variantRepository = variantRepository;
            _authService = authService;
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
            [FromQuery] int minPrice = 0,
            [FromQuery] int maxPrice = 999999999,
            [FromQuery] string? search = null,
            [FromQuery] bool? simplest = false
        )
        {
            try
            {
                // Retrieve total products and paginated products
                var (totalProducts, products) = await _productRepository.GetAllProducts(
                    pageNumber,
                    pageSize,
                    sortBy,
                    isAsc,
                    classificationId,
                    minPrice,
                    maxPrice,
                    search
                );

                // Return the full response
                return Ok(new
                {
                    TotalProducts = totalProducts,
                    Products = products
                });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { Message = "An error occurred while retrieving products.", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id, [FromQuery] string? includeVariant = null)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            // Calculate ReviewsDetail
            var totalReviews = product.Reviews.Count;
            var reviewsDetail = new
            {
                TotalReviews = totalReviews,
                OneStar = product.Reviews.Count(r => r.Rating == 1),
                OneStarPercent = totalReviews > 0 ? (double)product.Reviews.Count(r => r.Rating == 1) / totalReviews * 100 : 0,
                TwoStar = product.Reviews.Count(r => r.Rating == 2),
                TwoStarPercent = totalReviews > 0 ? (double)product.Reviews.Count(r => r.Rating == 2) / totalReviews * 100 : 0,
                ThreeStar = product.Reviews.Count(r => r.Rating == 3),
                ThreeStarPercent = totalReviews > 0 ? (double)product.Reviews.Count(r => r.Rating == 3) / totalReviews * 100 : 0,
                FourStar = product.Reviews.Count(r => r.Rating == 4),
                FourStarPercent = totalReviews > 0 ? (double)product.Reviews.Count(r => r.Rating == 4) / totalReviews * 100 : 0,
                FiveStar = product.Reviews.Count(r => r.Rating == 5),
                FiveStarPercent = totalReviews > 0 ? (double)product.Reviews.Count(r => r.Rating == 5) / totalReviews * 100 : 0,
                Reviews = product.Reviews.Select(r => new
                {
                    r.Id,
                    r.Rating,
                    r.Text,
                    r.CreatedAt,
                    Customer = new
                    {
                        r.Customer.Id,
                        r.Customer.Name,
                        r.Customer.Email
                    }
                }).ToList()
            };

            // Calculate Colors and Sizes
            var colors = product.Variants
            .SelectMany(v => v.VariantCategories)
            .Where(vc => vc.Category.ParentCategory != null && vc.Category.ParentCategory.Name.ToLower().Contains("color"))
            .GroupBy(vc => new { vc.Category.Id, vc.Category.Name, vc.Category.Description })
            .Select(g => new
            {
                g.Key.Id,
                g.Key.Name,
                g.Key.Description,
                Sizes = product.Variants
                    .SelectMany(v => v.VariantCategories)
                    .Where(vc => vc.Category.ParentCategory != null && vc.Category.ParentCategory.Name.ToLower().Contains("size"))
                    .GroupBy(vc => new { vc.Category.Id, vc.Category.Name })
                    .Select(sg => new
                    {
                        sg.Key.Id,
                        sg.Key.Name
                    }).ToList()
            }).ToList();

            // Include Variants if requested
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
                    v.UpdatedAt,
                    Color = v.VariantCategories?
                        .Where(vc => vc.Category != null &&
                                    vc.Category.ParentCategory != null &&
                                    vc.Category.ParentCategory.Name?.ToLower() == "color")
                        .Select(vc => new
                        {
                            Id = vc.Category?.Id ?? 0,
                            Name = vc.Category?.Name ?? string.Empty
                        })
                        .FirstOrDefault(),
                    Size = v.VariantCategories?
                        .Where
                        (
                            vc => vc.Category != null &&
                            vc.Category.ParentCategory != null &&
                            (vc.Category.ParentCategory.Name?.ToLower() == "jean size" || vc.Category.ParentCategory.Name?.ToLower() == "shirt size")
                        )
                        .Select(vc => new
                        {
                            Id = vc.Category?.Id ?? 0,
                            Name = vc.Category?.Name ?? string.Empty
                        })
                        .FirstOrDefault()
                }).ToList<object>();
            }
            return Ok(new
            {
                product.Id,
                product.Name,
                product.Price,
                product.Description,
                product.ImageUrl,
                Classifications = product.ProductClassifications.Select(pc => new
                {
                    pc.Classification.Id,
                    pc.Classification.Name
                }).ToList(),
                product.CreatedAt,
                product.UpdatedAt,
                AverageRating = product.Reviews != null && product.Reviews.Any()
                    ? product.Reviews.Average(r => r.Rating)
                    : 0,
                TotalOrders = product.Variants?
                        .SelectMany(v => v.VariantOrders ?? Enumerable.Empty<object>())
                        .Count() ?? 0,
                Variants = variants,
                ReviewDetails = reviewsDetail,
                Colors = colors
            });
        }
        
        [HttpPost]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyAdmin))]
        public async Task<IActionResult> CreateProduct
        (
            [FromForm] ProductDTO productDto, 
            [FromForm] string classifications,
            [FromForm] string variants
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
                var classificationIdList = classifications.Split(',');
                if (classificationIdList == null || classificationIdList.Length == 0) return BadRequest(new { Error = "Classification is required" });

                IList<CreateVariantsOfProductParameter> variantList = new List<CreateVariantsOfProductParameter>();
                Console.WriteLine(variants);
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
                foreach (var Id in classificationIdList) {
                    var classification = await _classificationRepository.GetClassificationById(int.Parse(Id));
                    if (classification == null) return BadRequest(new { Error = $"Classification with Id = {Id} not found"});
                    classificationList.Add(classification);
                }

                var product = new Product {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    Description = productDto.Description,
                    ImageUrl = productDto.ImageUrl
                };
                var productCreated = await _productRepository.CreateProduct(product, classificationList);

                if (variantList.Count > 0)
                {
                    foreach (var variantDto in variantList)
                    {
                        Console.WriteLine(variantDto.StockQuantity);
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
                            Price = int.Parse(variantDto.Price),
                            StockQuantity = int.Parse(variantDto.StockQuantity),
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
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyAdmin))]
        [ServiceFilter(typeof(UpdateAndDeleteProductFilter))]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductDTO productDto)
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
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyAdmin))]
        [ServiceFilter(typeof(UpdateAndDeleteProductFilter))]
        public async Task<IActionResult> DeleteProduct(int id, [FromBody] string password)
        {
            var admin = HttpContext.Items["admin"] as Admin;
            if (admin == null || !_authService.VerifyPassword(password, admin.Password))
            {
                return BadRequest("You do not have permission to perform this action.");
            }
            if (!await _productRepository.DeleteProduct(id))
            {
                return NotFound($"Product with ID {id} not found.");
            }

            return Ok($"Product with ID {id} deleted successfully.");
        }
    }
}