// using Xunit;
// using Moq;
// using System;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Ecommerce.BackendAPI.Controllers;
// using Ecommerce.BackendAPI.Interfaces;
// using Ecommerce.SharedViewModel.Models;
// using Ecommerce.SharedViewModel.DTOs;
// using System.Collections.Generic;
// using Microsoft.EntityFrameworkCore.Storage;
// using System.Linq;
// using Microsoft.AspNetCore.Http;
// using System.Text.Json;
// using Ecommerce.SharedViewModel.ParametersType;
// using Ecommerce.SharedViewModel.Responses;
// using Microsoft.AspNetCore.Http.HttpResults;

// namespace Ecommerce.BackendAPI.Tests
// {
//     public class ProductControllerTests
//     {
//         private readonly Mock<IProductRepository> _mockProductRepository;
//         private readonly Mock<IVariantRepository> _mockVariantRepository;
//         private readonly Mock<IAuthService> _mockAuthService;
//         private readonly Mock<IClassificationRepository> _mockClassificationRepository;
//         private readonly ProductController _controller;
//         private readonly Mock<IDbContextTransaction> _mockTransaction;

//         public ProductControllerTests()
//         {
//             _mockProductRepository = new Mock<IProductRepository>();
//             _mockVariantRepository = new Mock<IVariantRepository>();
//             _mockAuthService = new Mock<IAuthService>();
//             _mockClassificationRepository = new Mock<IClassificationRepository>();
//             _mockTransaction = new Mock<IDbContextTransaction>();

//             _controller = new ProductController(
//                 _mockProductRepository.Object,
//                 _mockVariantRepository.Object,
//                 _mockAuthService.Object,
//                 _mockClassificationRepository.Object
//             );

//             // Setup HttpContext
//             var httpContext = new DefaultHttpContext();
//             httpContext.Items = new Dictionary<object, object?>();
//             _controller.ControllerContext = new ControllerContext
//             {
//                 HttpContext = httpContext
//             };

//             // Setup transaction
//             _mockProductRepository.Setup(m => m.BeginTransactionAsync())
//                 .ReturnsAsync(_mockTransaction.Object);
//         }

//         // CREATE PRODUCT TESTS
//         [Fact]
//         public async Task CreateProduct_WithValidData_ReturnsOkResult()
//         {
//             // Arrange
//             var productDto = new ProductDTO
//             {
//                 Name = "Test Product",
//                 Price = 100,
//                 Description = "Test Description",
//                 ImageUrl = ""
//             };

//             var classifications = "1,2";
//             var variants = "[{\"Key\":\"variant1\",\"SKU\":\"SKU001\",\"Price\":\"120\",\"StockQuantity\":\"10\",\"ImageUrl\":\"\",\"Categories\":[3,4]}]";
            
//             var uploadedUrls = new Dictionary<string, string>
//             {
//                 { "product", "http://example.com/product.jpg" },
//                 { "variant1", "http://example.com/variant1.jpg" }
//             };
//             _controller.HttpContext.Items["UploadedUrls"] = uploadedUrls;

//             var classification1 = new Classification { Id = 1, Name = "Category 1" };
//             var classification2 = new Classification { Id = 2, Name = "Category 2" };
            
//             _mockClassificationRepository.Setup(m => m.GetClassificationByIdAsync(1))
//                 .ReturnsAsync(classification1);
//             _mockClassificationRepository.Setup(m => m.GetClassificationByIdAsync(2))
//                 .ReturnsAsync(classification2);

//             var createdProduct = new Product
//             {
//                 Id = 1,
//                 Name = "Test Product",
//                 Price = 100,
//                 Description = "Test Description",
//                 ImageUrl = "http://example.com/product.jpg"
//             };

//             _mockProductRepository.Setup(m => m.CreateProductAsync(It.IsAny<Product>(), It.IsAny<IList<Classification>>()))
//                 .ReturnsAsync(createdProduct);
            
//             _mockVariantRepository.Setup(m => m.CreateVariantAsync(It.IsAny<Variant>()))
//                 .ReturnsAsync(true);

//             // Act
//             var result = await _controller.CreateProduct(productDto, classifications, variants);

//             // Assert
//             var okResult = Assert.IsType<OkObjectResult>(result);
//             Assert.Equal("Product created successfully.", okResult.Value);
//             _mockTransaction.Verify(m => m.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
//         }

//         [Fact]
//         public async Task CreateProduct_WithNullProductDto_ReturnsBadRequest()
//         {
//             // Arrange
//             ProductDTO productDto = null;
//             var classifications = "1,2";
//             var variants = "[]";

//             // Act
//             var result = await _controller.CreateProduct(productDto, classifications, variants);

//             // Assert
//             var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//             Assert.Equal("Product data or classification is null.", badRequestResult.Value);
//             _mockTransaction.Verify(m => m.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
//         }

//         [Theory]
//         [InlineData("", "Invalid classification")]
//         [InlineData("999", "Classification not found")]
//         public async Task CreateProduct_WithInvalidClassifications_ReturnsBadRequest(string classificationId, string expectedErrorType)
//         {
//             // Arrange
//             var productDto = new ProductDTO
//             {
//                 Name = "Test Product",
//                 Price = 100,
//                 Description = "Test Description",
//                 ImageUrl = ""
//             };

//             var classifications = classificationId;
//             var variants = "[]";

//             if (expectedErrorType == "Classification not found")
//             {
//                 _mockClassificationRepository.Setup(m => m.GetClassificationByIdAsync(999))
//                     .ReturnsAsync((Classification)null);
//             }

//             // Act
//             var result = await _controller.CreateProduct(productDto, classifications, variants);

//             // Assert
//             var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//             _mockTransaction.Verify(m => m.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
            
//             if (expectedErrorType == "Invalid classification")
//             {
//                 Assert.Contains("Classification is required", badRequestResult.Value.ToString());
//             }
//             else if (expectedErrorType == "Classification not found")
//             {
//                 Assert.Contains($"Classification with Id = {classificationId} not found", badRequestResult.Value.ToString());
//             }
//         }

//         // GET PRODUCTS TESTS
//         [Fact]
//         public async Task GetAllProducts_ReturnsCorrectPaginatedProducts()
//         {
//             // Arrange
//             var pageNumber = 1;
//             var pageSize = 10;
//             var totalProducts = 15;
//             var products = new List<GetAllProductsResponse>();
            
//             for (int i = 1; i <= pageSize; i++)
//             {
//                 products.Add(new GetAllProductsResponse
//                 {
//                     Id = i,
//                     Name = $"Product {i}",
//                     Price = i * 10,
//                     Description = $"Description {i}",
//                     AverageRating = 4.5,
//                     TotalOrders = 5,
//                     TotalReviews = 10
//                 });
//             }

//             _mockProductRepository.Setup(m => m.GetAllProductsAsync(
//                     pageNumber, pageSize, "UpdatedAt", true, null, 0, 999999999, null))
//                 .ReturnsAsync((totalProducts, products));

//             // Act
//             var result = await _controller.GetAllProducts();

//             // Assert
//             var okResult = Assert.IsType<OkObjectResult>(result);
//             var returnValue = Assert.IsType<dynamic>(okResult.Value);
            
//             // Use reflection to access the dynamic object properties
//             var returnedTotalProducts = (int)((dynamic)okResult.Value).TotalProducts;
//             var returnedProducts = (IEnumerable<GetAllProductsResponse>)((dynamic)okResult.Value).Products;

//             Assert.Equal(totalProducts, returnedTotalProducts);
//             Assert.Equal(pageSize, returnedProducts.Count());
//         }

//         [Fact]
//         public async Task GetAllProducts_WithSimplestOption_ReturnsSimplifiedProducts()
//         {
//             // Arrange
//             var pageNumber = 1;
//             var pageSize = 10;
//             var totalProducts = 15;
//             var products = new List<GetAllProductsResponse>();
            
//             for (int i = 1; i <= pageSize; i++)
//             {
//                 products.Add(new GetAllProductsResponse
//                 {
//                     Id = i,
//                     Name = $"Product {i}",
//                     Price = i * 10,
//                     Description = $"Description {i}",
//                     AverageRating = 4.5,
//                     TotalOrders = 5,
//                     TotalReviews = 10
//                 });
//             }

//             _mockProductRepository.Setup(m => m.GetAllProducts(
//                     pageNumber, pageSize, "UpdatedAt", true, null, 0, 999999999, null))
//                 .ReturnsAsync((totalProducts, products));

//             // Act
//             var result = await _controller.GetAllProducts(simplest: true);

//             // Assert
//             var okResult = Assert.IsType<OkObjectResult>(result);
            
//             // Extract dynamic property and assert on simplified response
//             var simpleProducts = ((dynamic)okResult.Value).Products;
//             Assert.NotNull(simpleProducts);
            
//             // Check the first item to ensure it's simplified
//             var firstProduct = simpleProducts[0];
//             Assert.NotNull(firstProduct.Id);
//             Assert.NotNull(firstProduct.Name);
            
//             // Verify that other properties don't exist on simplified response
//             var exception = Record.Exception(() => firstProduct.Description);
//             Assert.NotNull(exception);
//         }

//         [Theory]
//         [InlineData(1, 5, "Name", true)]
//         [InlineData(2, 3, "Price", false)]
//         public async Task GetAllProducts_WithDifferentParameters_ReturnsPaginatedAndSortedProducts(
//             int pageNumber, int pageSize, string sortBy, bool isAsc)
//         {
//             // Arrange
//             var totalProducts = 15;
//             var products = new List<GetAllProductsResponse>();
            
//             for (int i = 1; i <= pageSize; i++)
//             {
//                 products.Add(new GetAllProductsResponse
//                 {
//                     Id = i,
//                     Name = $"Product {i}",
//                     Price = i * 10,
//                     Description = $"Description {i}"
//                 });
//             }

//             _mockProductRepository.Setup(m => m.GetAllProducts(
//                     pageNumber, pageSize, sortBy, isAsc, null, 0, 999999999, null))
//                 .ReturnsAsync((totalProducts, products));

//             // Act
//             var result = await _controller.GetAllProducts(
//                 pageNumber: pageNumber, 
//                 pageSize: pageSize, 
//                 sortBy: sortBy, 
//                 isAsc: isAsc);

//             // Assert
//             var okResult = Assert.IsType<OkObjectResult>(result);
//             var returnValue = Assert.IsType<dynamic>(okResult.Value);
            
//             var returnedTotalProducts = (int)((dynamic)okResult.Value).TotalProducts;
//             var returnedProducts = (IEnumerable<GetAllProductsResponse>)((dynamic)okResult.Value).Products;

//             Assert.Equal(totalProducts, returnedTotalProducts);
//             Assert.Equal(pageSize, returnedProducts.Count());
//         }

//         [Theory]
//         [InlineData(1, null, "test")]
//         [InlineData(null, 100, 200)]
//         public async Task GetAllProducts_WithFilters_ReturnsFilteredProducts(
//             int? classificationId, int? minPrice, int? maxPrice)
//         {
//             // Arrange
//             var pageNumber = 1;
//             var pageSize = 10;
//             var totalProducts = 5;
//             var products = new List<GetAllProductsResponse>();
            
//             for (int i = 1; i <= 5; i++)
//             {
//                 products.Add(new GetAllProductsResponse
//                 {
//                     Id = i,
//                     Name = $"Product {i}",
//                     Price = i * 10,
//                     Description = $"Description {i}"
//                 });
//             }

//             _mockProductRepository.Setup(m => m.GetAllProducts(
//                     pageNumber, pageSize, "UpdatedAt", true, 
//                     classificationId, 
//                     minPrice ?? 0, 
//                     maxPrice ?? 999999999, 
//                     It.IsAny<string>()))
//                 .ReturnsAsync((totalProducts, products));

//             // Act
//             var result = await _controller.GetAllProducts(
//                 classificationId: classificationId,
//                 minPrice: minPrice ?? 0,
//                 maxPrice: maxPrice ?? 999999999,
//                 search: "test");

//             // Assert
//             var okResult = Assert.IsType<OkObjectResult>(result);
//             var returnValue = Assert.IsType<dynamic>(okResult.Value);
            
//             var returnedTotalProducts = (int)((dynamic)okResult.Value).TotalProducts;
//             Assert.Equal(totalProducts, returnedTotalProducts);
//         }

//         // GET PRODUCT BY ID TESTS
//         [Fact]
//         public async Task GetProductById_WithExistingId_ReturnsProduct()
//         {
//             // Arrange
//             var productId = 1;
//             var product = new Product
//             {
//                 Id = productId,
//                 Name = "Test Product",
//                 Price = 100,
//                 Description = "Test Description",
//                 ImageUrl = "http://example.com/product.jpg",
//                 Reviews = new List<Review>
//                 {
//                     new Review { Id = 1, Rating = 5, Customer = new Customer { Id = 1, Name = "Test Customer", Email = "test@example.com" } },
//                     new Review { Id = 2, Rating = 4, Customer = new Customer { Id = 2, Name = "Test Customer 2", Email = "test2@example.com" } }
//                 },
//                 Variants = new List<Variant>(),
//                 ProductClassifications = new List<ProductClassification>
//                 {
//                     new ProductClassification 
//                     { 
//                         Classification = new Classification { Id = 1, Name = "Category 1" } 
//                     }
//                 }
//             };

//             _mockProductRepository.Setup(m => m.GetProductById(productId))
//                 .ReturnsAsync(product);

//             // Act
//             var result = await _controller.GetProductById(productId);

//             // Assert
//             var okResult = Assert.IsType<OkObjectResult>(result);
//             Assert.NotNull(okResult.Value);
            
//             // Check some properties to ensure correct mapping
//             Assert.Equal(productId, ((dynamic)okResult.Value).Id);
//             Assert.Equal("Test Product", ((dynamic)okResult.Value).Name);
//             Assert.Equal(100, ((dynamic)okResult.Value).Price);
//         }

//         [Theory]
//         [InlineData(999)]
//         public async Task GetProductById_WithNonExistingId_ReturnsNotFound(int productId)
//         {
//             // Arrange
//             _mockProductRepository.Setup(m => m.GetProductById(productId))
//                 .ReturnsAsync((Product)null);

//             // Act
//             var result = await _controller.GetProductById(productId);

//             // Assert
//             Assert.IsType<NotFoundResult>(result);
//         }

//         // DELETE PRODUCT TESTS
//         [Fact]
//         public async Task DeleteProduct_WithValidIdAndPassword_ReturnsOkResult()
//         {
//             // Arrange
//             var productId = 1;
//             var password = "admin_password";
//             var hashedPassword = "hashed_password";
            
//             var admin = new Admin
//             {
//                 Id = 1,
//                 Name = "Admin",
//                 Email = "admin@example.com",
//                 Password = hashedPassword
//             };
//             _controller.HttpContext.Items["admin"] = admin;
            
//             _mockAuthService.Setup(m => m.VerifyPassword(password, hashedPassword))
//                 .Returns(true);
                
//             _mockProductRepository.Setup(m => m.DeleteProduct(productId))
//                 .ReturnsAsync(true);

//             // Act
//             var result = await _controller.DeleteProduct(productId, password);

//             // Assert
//             var okResult = Assert.IsType<OkObjectResult>(result);
//             Assert.Equal($"Product with ID {productId} deleted successfully.", okResult.Value);
//         }

//         [Fact]
//         public async Task DeleteProduct_WithInvalidPassword_ReturnsBadRequest()
//         {
//             // Arrange
//             var productId = 1;
//             var password = "wrong_password";
//             var hashedPassword = "hashed_password";
            
//             var admin = new Admin
//             {
//                 Id = 1,
//                 Name = "Admin",
//                 Email = "admin@example.com",
//                 Password = hashedPassword
//             };
//             _controller.HttpContext.Items["admin"] = admin;
            
//             _mockAuthService.Setup(m => m.VerifyPassword(password, hashedPassword))
//                 .Returns(false);

//             // Act
//             var result = await _controller.DeleteProduct(productId, password);

//             // Assert
//             var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//             Assert.Equal("You do not have permission to perform this action.", badRequestResult.Value);
//         }

//         [Theory]
//         [InlineData(999)]
//         public async Task DeleteProduct_WithNonExistingId_ReturnsNotFound(int productId)
//         {
//             // Arrange
//             var password = "admin_password";
//             var hashedPassword = "hashed_password";
            
//             var admin = new Admin
//             {
//                 Id = 1,
//                 Name = "Admin",
//                 Email = "admin@example.com",
//                 Password = hashedPassword
//             };
//             _controller.HttpContext.Items["admin"] = admin;
            
//             _mockAuthService.Setup(m => m.VerifyPassword(password, hashedPassword))
//                 .Returns(true);
                
//             _mockProductRepository.Setup(m => m.DeleteProduct(productId))
//                 .ReturnsAsync(false);

//             // Act
//             var result = await _controller.DeleteProduct(productId, password);

//             // Assert
//             var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
//             Assert.Equal($"Product with ID {productId} not found.", notFoundResult.Value);
//         }
//     }
// }