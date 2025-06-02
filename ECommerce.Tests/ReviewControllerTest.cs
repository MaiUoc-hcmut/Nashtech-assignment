using Ecommerce.BackendAPI.Controllers;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using Ecommerce.SharedViewModel.Responses;

namespace Ecommerce.Tests
{
    public class ReviewControllerTests
    {
        private readonly Mock<IReviewRepository> _mockReviewRepository;
        private readonly ReviewController _controller;

        public ReviewControllerTests()
        {
            _mockReviewRepository = new Mock<IReviewRepository>();
            _controller = new ReviewController(_mockReviewRepository.Object);

            // Setup the HTTP context for controller
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        #region GetAllReviews Tests

        [Fact]
        public async Task GetAllReviews_ReturnsOk_WithReviewsList()
        {
            // Arrange
            var reviews = GetTestReviews();
            var TotalReviews = reviews.Count;
            
            _mockReviewRepository.Setup(repo => repo.GetReviewsAsync(
                It.IsAny<List<int>>(),
                It.IsAny<int>(),
                It.IsAny<double>(),
                It.IsAny<double>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<string>(),
                It.IsAny<bool>()
            )).ReturnsAsync((TotalReviews, reviews));

            // Act
            var result = await _controller.GetAllReviews(
                new List<int>(),
                1,
                0,
                5,
                null,
                null,
                "CreatedAt",
                true
            );

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseObj = okResult.Value as dynamic;
            Assert.Equal(TotalReviews, responseObj.TotalReviews);
            Assert.NotNull(responseObj.Reviews);
        }

        [Fact]
        public async Task GetAllReviews_FiltersWithProductIds_ReturnsFilteredReviews()
        {
            // Arrange
            var productIds = new List<int> { 1, 2 };
            var reviews = GetTestReviews().Where(r => productIds.Contains(r.Product.Id)).ToList();
            var totalReviews = reviews.Count;
            
            _mockReviewRepository.Setup(repo => repo.GetReviewsAsync(
                productIds,
                It.IsAny<int>(),
                It.IsAny<double>(),
                It.IsAny<double>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<string>(),
                It.IsAny<bool>()
            )).ReturnsAsync((totalReviews, reviews));

            // Act
            var result = await _controller.GetAllReviews(
                productIds,
                1,
                0,
                5,
                null,
                null,
                "CreatedAt",
                true
            );

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseObj = okResult.Value as dynamic;
            Assert.Equal(totalReviews, responseObj.TotalReviews);
        }

        [Fact]
        public async Task GetAllReviews_WithExceptionThrown_Returns500StatusCode()
        {
            // Arrange
            _mockReviewRepository.Setup(repo => repo.GetReviewsAsync(
                It.IsAny<List<int>>(),
                It.IsAny<int>(),
                It.IsAny<double>(),
                It.IsAny<double>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<string>(),
                It.IsAny<bool>()
            )).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetAllReviews(
                new List<int>(),
                1,
                0,
                5,
                null,
                null,
                "CreatedAt",
                true
            );

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Theory]
        [InlineData(1, 1, 3, "CreatedAt", false)]
        [InlineData(2, 2, 5, "Rating", true)]
        public async Task GetAllReviews_WithDifferentParameters_ReturnsCorrectData(
            int pageNumber, double minRating, double maxRating, string sortBy, bool isAsc)
        {
            // Arrange
            var reviews = GetTestReviews();
            var totalReviews = reviews.Count;
            
            _mockReviewRepository.Setup(repo => repo.GetReviewsAsync(
                It.IsAny<List<int>>(),
                pageNumber,
                minRating,
                maxRating,
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                sortBy,
                isAsc
            )).ReturnsAsync((totalReviews, reviews));

            // Act
            var result = await _controller.GetAllReviews(
                new List<int>(),
                pageNumber,
                minRating,
                maxRating,
                null,
                null,
                sortBy,
                isAsc
            );

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Theory]
        [InlineData(null, "2025-12-31")]
        [InlineData("2025-01-01", null)]
        public async Task GetAllReviews_WithOptionalDateParameters_SetsDefaultValues(
            string startDateStr, string endDateStr)
        {
            // Arrange
            DateTime? startDate = startDateStr != null ? DateTime.Parse(startDateStr) : null;
            DateTime? endDate = endDateStr != null ? DateTime.Parse(endDateStr) : null;
            
            var reviews = GetTestReviews();
            var totalReviews = reviews.Count;
            
            _mockReviewRepository.Setup(repo => repo.GetReviewsAsync(
                It.IsAny<List<int>>(),
                It.IsAny<int>(),
                It.IsAny<double>(),
                It.IsAny<double>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<string>(),
                It.IsAny<bool>()
            )).ReturnsAsync((totalReviews, reviews));

            // Act
            var result = await _controller.GetAllReviews(
                new List<int>(),
                1,
                0,
                5,
                startDate,
                endDate,
                "CreatedAt",
                true
            );

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        #endregion

       #region GetReviewsByProductId Tests

        [Fact]
        public async Task GetReviewsByProductId_ReturnsOk_WithReviewsList()
        {
            // Arrange
            int productId = 1;
            var reviews = GetTestReviews().Where(r => r.Product.Id == productId).ToList();
            
            _mockReviewRepository.Setup(repo => repo.GetReviewsByProductIdAsync(productId))
                .ReturnsAsync(reviews);

            // Act
            var result = await _controller.GetReviewsByProductId(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseObj = okResult.Value as dynamic;
            Assert.NotNull(responseObj.Reviews);
            Assert.NotNull(responseObj.AverageRating);
            Assert.NotNull(responseObj.TotalReviews);
            Assert.NotNull(responseObj.RatingsCount);
        }

        [Fact]
        public async Task GetReviewsByProductId_CalculatesAverageRating_Correctly()
        {
            // Arrange
            int productId = 1;
            var reviews = new List<Review>
            {
                new Review 
                { 
                    Id = 1, 
                    Rating = 4, 
                    Text = "Great", 
                    Product = new Product 
                    { 
                        Id = productId, 
                        Name = "Test Product", 
                        Description = "Description for Test Product",
                        ImageUrl = "test.jpg"
                    }, 
                    Customer = new Customer 
                    { 
                        Id = 1, 
                        Name = "Customer 1", 
                        Email = "customer1@example.com", 
                        Username = "customer1", 
                        Password = "password123" 
                    } 
                },
                new Review 
                { 
                    Id = 2, 
                    Rating = 5, 
                    Text = "Excellent", 
                    Product = new Product 
                    { 
                        Id = productId, 
                        Name = "Test Product", 
                        Description = "Description for Test Product",
                        ImageUrl = "test.jpg"
                    }, 
                    Customer = new Customer 
                    { 
                        Id = 2, 
                        Name = "Customer 2", 
                        Email = "customer2@example.com", 
                        Username = "customer2", 
                        Password = "password123" 
                    } 
                },
                new Review 
                { 
                    Id = 3, 
                    Rating = 3, 
                    Text = "Good", 
                    Product = new Product 
                    { 
                        Id = productId, 
                        Name = "Test Product", 
                        Description = "Description for Test Product",
                        ImageUrl = "test.jpg"
                    }, 
                    Customer = new Customer 
                    { 
                        Id = 3, 
                        Name = "Customer 3", 
                        Email = "customer3@example.com", 
                        Username = "customer3", 
                        Password = "password123" 
                    } 
                }
            };
            
            double expectedAverage = reviews.Average(r => r.Rating);
            
            _mockReviewRepository.Setup(repo => repo.GetReviewsByProductIdAsync(productId))
                .ReturnsAsync(reviews);

            // Act
            var result = await _controller.GetReviewsByProductId(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseObj = okResult.Value as dynamic;
            Assert.Equal(expectedAverage, responseObj.AverageRating);
        }

        [Fact]
        public async Task GetReviewsByProductId_CalculatesRatingsCount_Correctly()
        {
            // Arrange
            int productId = 1;
            var reviews = new List<Review>
            {
                new Review 
                { 
                    Id = 1, 
                    Rating = 4, 
                    Text = "Great", 
                    Product = new Product 
                    { 
                        Id = productId, 
                        Name = "Test Product", 
                        Description = "Description for Test Product",
                        ImageUrl = "test.jpg"
                    }, 
                    Customer = new Customer 
                    { 
                        Id = 1, 
                        Name = "Customer 1", 
                        Email = "customer1@example.com", 
                        Username = "customer1", 
                        Password = "password123" 
                    } 
                },
                new Review 
                { 
                    Id = 2, 
                    Rating = 5, 
                    Text = "Excellent", 
                    Product = new Product 
                    { 
                        Id = productId, 
                        Name = "Test Product", 
                        Description = "Description for Test Product",
                        ImageUrl = "test.jpg"
                    }, 
                    Customer = new Customer 
                    { 
                        Id = 2, 
                        Name = "Customer 2", 
                        Email = "customer2@example.com", 
                        Username = "customer2", 
                        Password = "password123" 
                    } 
                },
                new Review 
                { 
                    Id = 3, 
                    Rating = 4, 
                    Text = "Good", 
                    Product = new Product 
                    { 
                        Id = productId, 
                        Name = "Test Product", 
                        Description = "Description for Test Product",
                        ImageUrl = "test.jpg"
                    }, 
                    Customer = new Customer 
                    { 
                        Id = 3, 
                        Name = "Customer 3", 
                        Email = "customer3@example.com", 
                        Username = "customer3", 
                        Password = "password123" 
                    } 
                }
            };
            
            var expectedRatingsCount = new Dictionary<int, int>
            {
                { 4, 2 },
                { 5, 1 }
            };
            
            _mockReviewRepository.Setup(repo => repo.GetReviewsByProductIdAsync(productId))
                .ReturnsAsync(reviews);

            // Act
            var result = await _controller.GetReviewsByProductId(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseObj = okResult.Value as dynamic;
            var ratingsCount = responseObj.RatingsCount as Dictionary<int, int>;
            Assert.Equal(expectedRatingsCount[4], ratingsCount[4]);
            Assert.Equal(expectedRatingsCount[5], ratingsCount[5]);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetReviewsByProductId_WithDifferentProductIds_ReturnsCorrectData(int productId)
        {
            // Arrange
            var reviews = GetTestReviews().Where(r => r.Product.Id == productId).ToList();
            
            _mockReviewRepository.Setup(repo => repo.GetReviewsByProductIdAsync(productId))
                .ReturnsAsync(reviews);

            // Act
            var result = await _controller.GetReviewsByProductId(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(999)]
        public async Task GetReviewsByProductId_WithNoReviews_ReturnsEmptyList(int productId)
        {
            // Arrange
            var emptyReviews = new List<Review>();
            
            _mockReviewRepository.Setup(repo => repo.GetReviewsByProductIdAsync(productId))
                .ReturnsAsync(emptyReviews);

            // Act
            var result = await _controller.GetReviewsByProductId(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseObj = okResult.Value as GetReviewsByProductResponse;
            Assert.Equal(0, responseObj.TotalReviews);
            Assert.Equal(0, responseObj.AverageRating);
        }

        #endregion



        #region Helper Methods
        private List<Review> GetTestReviews()
        {
            return new List<Review>
            {
                new Review 
                { 
                    Id = 1, 
                    Rating = 5, 
                    Text = "Excellent product, highly recommend!", 
                    CreatedAt = new DateTime(2025, 1, 15),
                    Product = new Product 
                    { 
                        Id = 1, 
                        Name = "Product 1", 
                        Description = "Description for Product 1",
                        ImageUrl = "image1.jpg"
                    },
                    Customer = new Customer 
                    { 
                        Id = 1, 
                        Name = "John Doe",
                        Email = "john.doe@example.com",
                        Username = "johndoe",
                        Password = "password123"
                    }
                },
                new Review 
                { 
                    Id = 2, 
                    Rating = 4, 
                    Text = "Good product, but could be better.", 
                    CreatedAt = new DateTime(2025, 2, 20),
                    Product = new Product 
                    { 
                        Id = 2, 
                        Name = "Product 2", 
                        Description = "Description for Product 2",
                        ImageUrl = "image2.jpg"
                    },
                    Customer = new Customer 
                    { 
                        Id = 2, 
                        Name = "Jane Smith",
                        Email = "jane.smith@example.com",
                        Username = "janesmith",
                        Password = "password123"
                    }
                },
                new Review 
                { 
                    Id = 3, 
                    Rating = 3, 
                    Text = "Average quality.", 
                    CreatedAt = new DateTime(2025, 3, 10),
                    Product = new Product 
                    { 
                        Id = 1, 
                        Name = "Product 1", 
                        Description = "Description for Product 1",
                        ImageUrl = "image1.jpg"
                    },
                    Customer = new Customer 
                    { 
                        Id = 3, 
                        Name = "Bob Johnson",
                        Email = "bob.johnson@example.com",
                        Username = "bobjohnson",
                        Password = "password123"
                    }
                },
                new Review 
                { 
                    Id = 4, 
                    Rating = 5, 
                    Text = "Best purchase ever!", 
                    CreatedAt = new DateTime(2025, 4, 5),
                    Product = new Product 
                    { 
                        Id = 3, 
                        Name = "Product 3", 
                        Description = "Description for Product 3",
                        ImageUrl = "image3.jpg"
                    },
                    Customer = new Customer 
                    { 
                        Id = 1, 
                        Name = "John Doe",
                        Email = "john.doe@example.com",
                        Username = "johndoe",
                        Password = "password123"
                    }
                },
                new Review 
                { 
                    Id = 5, 
                    Rating = 2, 
                    Text = "Disappointed with quality.", 
                    CreatedAt = new DateTime(2025, 5, 1),
                    Product = new Product 
                    { 
                        Id = 2, 
                        Name = "Product 2", 
                        Description = "Description for Product 2",
                        ImageUrl = "image2.jpg"
                    },
                    Customer = new Customer 
                    { 
                        Id = 4, 
                        Name = "Alice Brown",
                        Email = "alice.brown@example.com",
                        Username = "alicebrown",
                        Password = "password123"
                    }
                }
            };
        }

        #endregion
    }
}