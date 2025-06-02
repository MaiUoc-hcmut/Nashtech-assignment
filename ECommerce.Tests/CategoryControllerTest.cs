using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce.BackendAPI.Controllers;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Ecommerce.BackendAPI.Tests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<IParentCategoryRepo> _mockParentCategoryRepo;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockParentCategoryRepo = new Mock<IParentCategoryRepo>();
            _controller = new CategoryController(_mockCategoryRepository.Object, _mockParentCategoryRepo.Object);
        }

        #region GetAllCategories Tests

        [Fact]
        public async Task GetAllCategories_ReturnsOkResult_WhenCategoriesExist()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics", Description = "Electronic items" },
                new Category { Id = 2, Name = "Clothing", Description = "Clothing items" }
            };
            _mockCategoryRepository.Setup(repo => repo.GetAllCategoriesAsync()).ReturnsAsync(categories);

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategories = Assert.IsAssignableFrom<List<Category>>(okResult.Value);
            Assert.Equal(2, returnedCategories.Count);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsOkResult_WithEmptyList_WhenNoCategoriesExist()
        {
            // Arrange
            var categories = new List<Category>();
            _mockCategoryRepository.Setup(repo => repo.GetAllCategoriesAsync()).ReturnsAsync(categories);

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategories = Assert.IsAssignableFrom<List<Category>>(okResult.Value);
            Assert.Empty(returnedCategories);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(3, 4)]
        public async Task GetAllCategories_ReturnsCorrectNumberOfCategories(int firstId, int secondId)
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = firstId, Name = "Category1", Description = "Description1" },
                new Category { Id = secondId, Name = "Category2", Description = "Description2" }
            };
            _mockCategoryRepository.Setup(repo => repo.GetAllCategoriesAsync()).ReturnsAsync(categories);

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategories = Assert.IsAssignableFrom<List<Category>>(okResult.Value);
            Assert.Equal(2, returnedCategories.Count);
            Assert.Contains(returnedCategories, c => c.Id == firstId);
            Assert.Contains(returnedCategories, c => c.Id == secondId);
        }

        [Theory]
        [InlineData("Electronics", "Clothing")]
        [InlineData("Books", "Toys")]
        public async Task GetAllCategories_ReturnsCorrectCategoryNames(string firstName, string secondName)
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = firstName, Description = "Description1" },
                new Category { Id = 2, Name = secondName, Description = "Description2" }
            };
            _mockCategoryRepository.Setup(repo => repo.GetAllCategoriesAsync()).ReturnsAsync(categories);

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategories = Assert.IsAssignableFrom<List<Category>>(okResult.Value);
            Assert.Contains(returnedCategories, c => c.Name == firstName);
            Assert.Contains(returnedCategories, c => c.Name == secondName);
        }

        #endregion

        #region GetCategoryById Tests

        [Fact]
        public async Task GetCategoryById_ReturnsOkResult_WhenCategoryExists()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Electronics", Description = "Electronic items" };
            _mockCategoryRepository.Setup(repo => repo.GetCategoryByIdAsync(categoryId)).ReturnsAsync(category);

            // Act
            var result = await _controller.GetCategoryById(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategory = Assert.IsType<Category>(okResult.Value);
            Assert.Equal(categoryId, returnedCategory.Id);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            var categoryId = 1;
            _mockCategoryRepository.Setup(repo => repo.GetCategoryByIdAsync(categoryId)).ReturnsAsync((Category)null);

            // Act
            var result = await _controller.GetCategoryById(categoryId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Category not found", notFoundResult.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetCategoryById_ReturnsCorrectCategory_WhenIdIsValid(int categoryId)
        {
            // Arrange
            var category = new Category { Id = categoryId, Name = $"Category{categoryId}", Description = $"Description{categoryId}" };
            _mockCategoryRepository.Setup(repo => repo.GetCategoryByIdAsync(categoryId)).ReturnsAsync(category);

            // Act
            var result = await _controller.GetCategoryById(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategory = Assert.IsType<Category>(okResult.Value);
            Assert.Equal(categoryId, returnedCategory.Id);
            Assert.Equal($"Category{categoryId}", returnedCategory.Name);
            Assert.Equal($"Description{categoryId}", returnedCategory.Description);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(999)]
        public async Task GetCategoryById_ReturnsNotFound_WhenIdIsInvalid(int categoryId)
        {
            // Arrange
            _mockCategoryRepository.Setup(repo => repo.GetCategoryByIdAsync(categoryId)).ReturnsAsync((Category)null);

            // Act
            var result = await _controller.GetCategoryById(categoryId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Category not found", notFoundResult.Value);
        }

        #endregion

        #region GetCategoriesByParentId Tests

        [Fact]
        public async Task GetCategoriesByParentId_ReturnsOkResult_WhenCategoriesExist()
        {
            // Arrange
            var parentId = 1;
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics", Description = "Electronic items" },
                new Category { Id = 2, Name = "Phones", Description = "Phone items" }
            };
            _mockCategoryRepository.Setup(repo => repo.GetCategoriesByParentIdAsync(parentId)).ReturnsAsync(categories);

            // Act
            var result = await _controller.GetCategoriesByParentId(parentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategories = Assert.IsAssignableFrom<List<Category>>(okResult.Value);
            Assert.Equal(2, returnedCategories.Count);
        }

        [Fact]
        public async Task GetCategoriesByParentId_ReturnsNotFound_WhenNoCategoriesExist()
        {
            // Arrange
            var parentId = 1;
            var categories = new List<Category>();
            _mockCategoryRepository.Setup(repo => repo.GetCategoriesByParentIdAsync(parentId)).ReturnsAsync(categories);

            // Act
            var result = await _controller.GetCategoriesByParentId(parentId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No categories found for this parent ID", notFoundResult.Value);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(3, 3)]
        public async Task GetCategoriesByParentId_ReturnsCorrectNumberOfCategories(int parentId, int expectedCount)
        {
            // Arrange
            var categories = new List<Category>();
            for (int i = 1; i <= expectedCount; i++)
            {
                categories.Add(new Category { Id = i, Name = $"Category{i}", Description = $"Description{i}" });
            }
            _mockCategoryRepository.Setup(repo => repo.GetCategoriesByParentIdAsync(parentId)).ReturnsAsync(categories);

            // Act
            var result = await _controller.GetCategoriesByParentId(parentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategories = Assert.IsAssignableFrom<List<Category>>(okResult.Value);
            Assert.Equal(expectedCount, returnedCategories.Count);
        }

        [Theory]
        [InlineData(1, null)]
        [InlineData(2, new int[] {})]
        public async Task GetCategoriesByParentId_ReturnsNotFound_WhenCategoriesAreNull(int parentId, int[] dummyData)
        {
            // Arrange
            List<Category> categories = null;
            if (dummyData != null)
            {
                categories = new List<Category>();
            }
            _mockCategoryRepository.Setup(repo => repo.GetCategoriesByParentIdAsync(parentId)).ReturnsAsync(categories);

            // Act
            var result = await _controller.GetCategoriesByParentId(parentId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No categories found for this parent ID", notFoundResult.Value);
        }

        #endregion

        #region SearchCategoryByPattern Tests

        [Fact]
        public async Task SearchCategoryByPattern_ReturnsOkResult_WhenCategoriesMatch()
        {
            // Arrange
            var pattern = "elec";
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics", Description = "Electronic items" },
                new Category { Id = 2, Name = "Electrical", Description = "Electrical items" }
            };
            _mockCategoryRepository.Setup(repo => repo.SearchCategoryByPatternAsync(pattern)).ReturnsAsync(categories);

            // Act
            var result = await _controller.SearchCategoryByPattern(pattern);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategories = Assert.IsAssignableFrom<IEnumerable<Category>>(okResult.Value);
            Assert.Equal(2, returnedCategories.Count());
        }

        [Fact]
        public async Task SearchCategoryByPattern_ReturnsEmptyList_WhenNoMatch()
        {
            // Arrange
            var pattern = "xyz";
            var categories = new List<Category>();
            _mockCategoryRepository.Setup(repo => repo.SearchCategoryByPatternAsync(pattern)).ReturnsAsync(categories);

            // Act
            var result = await _controller.SearchCategoryByPattern(pattern);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategories = Assert.IsAssignableFrom<IEnumerable<Category>>(okResult.Value);
            Assert.Empty(returnedCategories);
        }

        [Theory]
        [InlineData("elec", 2)]
        [InlineData("cloth", 1)]
        [InlineData("xyz", 0)]
        public async Task SearchCategoryByPattern_ReturnsCorrectNumberOfMatches(string pattern, int expectedCount)
        {
            // Arrange
            var categories = new List<Category>();
            if (pattern == "elec")
            {
                categories.Add(new Category { Id = 1, Name = "Electronics", Description = "Description1" });
                categories.Add(new Category { Id = 2, Name = "Electrical", Description = "Description2" });
            }
            else if (pattern == "cloth")
            {
                categories.Add(new Category { Id = 3, Name = "Clothing", Description = "Description3" });
            }
            _mockCategoryRepository.Setup(repo => repo.SearchCategoryByPatternAsync(pattern)).ReturnsAsync(categories);

            // Act
            var result = await _controller.SearchCategoryByPattern(pattern);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategories = Assert.IsAssignableFrom<IEnumerable<Category>>(okResult.Value);
            Assert.Equal(expectedCount, returnedCategories.Count());
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData(" ", 0)]
        [InlineData(null, 0)]
        public async Task SearchCategoryByPattern_HandlesEmptyOrNullPattern(string pattern, int expectedCount)
        {
            // Arrange
            var categories = new List<Category>();
            _mockCategoryRepository.Setup(repo => repo.SearchCategoryByPatternAsync(pattern ?? "")).ReturnsAsync(categories);

            // Act
            var result = await _controller.SearchCategoryByPattern(pattern);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            // Use IEnumerable<Category> instead of List<Category> to handle both List<Category> and Category[]
            var returnedCategories = Assert.IsAssignableFrom<IEnumerable<Category>>(okResult.Value);
            Assert.Equal(expectedCount, returnedCategories.Count());
        }

        #endregion

        #region CreateCategory Tests

        [Fact]
        public async Task CreateCategory_ReturnsOkResult_WhenCategoryIsCreated()
        {
            // Arrange
            var parentId = 1;
            var categoryDto = new CategoryDTO { Name = "Electronics", Description = "Electronic items" };
            var parentCategory = new ParentCategory { Id = parentId, Name = "Main Category" };
            var createdCategory = new Category { Id = 1, Name = "Electronics", Description = "Electronic items", ParentCategory = parentCategory };
            
            _mockParentCategoryRepo.Setup(repo => repo.GetParentCategoryByIdAsync(parentId)).ReturnsAsync(parentCategory);
            _mockCategoryRepository.Setup(repo => repo.CreateCategoryAsync(categoryDto, parentCategory)).ReturnsAsync(createdCategory);

            // Act
            var result = await _controller.CreateCategory(categoryDto, parentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategory = Assert.IsType<Category>(okResult.Value);
            Assert.Equal(categoryDto.Name, returnedCategory.Name);
            Assert.Equal(categoryDto.Description, returnedCategory.Description);
        }

        [Fact]
        public async Task CreateCategory_ReturnsBadRequest_WhenCategoryDtoIsNull()
        {
            // Arrange
            var parentId = 1;
            CategoryDTO categoryDto = null;

            // Act
            var result = await _controller.CreateCategory(categoryDto, parentId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid category data", badRequestResult.Value);
        }

        [Theory]
        [InlineData("Electronics", "Electronic items", 1)]
        [InlineData("Clothing", "Clothing items", 2)]
        public async Task CreateCategory_CreatesCategory_WithCorrectData(string name, string description, int parentId)
        {
            // Arrange
            var categoryDto = new CategoryDTO { Name = name, Description = description };
            var parentCategory = new ParentCategory { Id = parentId, Name = $"Parent{parentId}" };
            var createdCategory = new Category { 
                Id = 1, 
                Name = name, 
                Description = description, 
                ParentCategory = parentCategory 
            };
            
            _mockParentCategoryRepo.Setup(repo => repo.GetParentCategoryByIdAsync(parentId)).ReturnsAsync(parentCategory);
            _mockCategoryRepository.Setup(repo => repo.CreateCategoryAsync(categoryDto, parentCategory)).ReturnsAsync(createdCategory);

            // Act
            var result = await _controller.CreateCategory(categoryDto, parentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategory = Assert.IsType<Category>(okResult.Value);
            Assert.Equal(name, returnedCategory.Name);
            Assert.Equal(description, returnedCategory.Description);
            Assert.Equal(parentId, returnedCategory.ParentCategory.Id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(999)]
        public async Task CreateCategory_ReturnsNotFound_WhenParentCategoryDoesNotExist(int parentId)
        {
            // Arrange
            var categoryDto = new CategoryDTO { Name = "Electronics", Description = "Electronic items" };
            
            _mockParentCategoryRepo.Setup(repo => repo.GetParentCategoryByIdAsync(parentId)).ReturnsAsync((ParentCategory)null);

            // Act
            var result = await _controller.CreateCategory(categoryDto, parentId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Parent category not found", notFoundResult.Value);
        }

        #endregion

        #region UpdateCategory Tests

        [Fact]
        public async Task UpdateCategory_ReturnsOkResult_WhenCategoryIsUpdated()
        {
            // Arrange
            var categoryId = 1;
            var categoryDto = new CategoryDTO { Name = "Updated Electronics", Description = "Updated Electronic items" };
            var updatedCategory = new Category { Id = categoryId, Name = "Updated Electronics", Description = "Updated Electronic items" };
            
            _mockCategoryRepository.Setup(repo => repo.UpdateCategoryAsync(It.IsAny<CategoryDTO>())).ReturnsAsync(updatedCategory);

            // Act
            var result = await _controller.UpdateCategory(categoryId, categoryDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategory = Assert.IsType<Category>(okResult.Value);
            Assert.Equal(categoryDto.Name, returnedCategory.Name);
            Assert.Equal(categoryDto.Description, returnedCategory.Description);
        }

        [Fact]
        public async Task UpdateCategory_ReturnsBadRequest_WhenCategoryDtoIsNull()
        {
            // Arrange
            var categoryId = 1;
            CategoryDTO categoryDto = null;

            // Act
            var result = await _controller.UpdateCategory(categoryId, categoryDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid category data", badRequestResult.Value);
        }

        [Theory]
        [InlineData(1, "Updated Electronics", "Updated Electronic items")]
        [InlineData(2, "Updated Clothing", "Updated Clothing items")]
        public async Task UpdateCategory_UpdatesCategory_WithCorrectData(int categoryId, string name, string description)
        {
            // Arrange
            var categoryDto = new CategoryDTO { Name = name, Description = description };
            var updatedCategory = new Category { Id = categoryId, Name = name, Description = description };
            
            _mockCategoryRepository.Setup(repo => repo.UpdateCategoryAsync(It.IsAny<CategoryDTO>())).ReturnsAsync(updatedCategory);

            // Act
            var result = await _controller.UpdateCategory(categoryId, categoryDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategory = Assert.IsType<Category>(okResult.Value);
            Assert.Equal(categoryId, returnedCategory.Id);
            Assert.Equal(name, returnedCategory.Name);
            Assert.Equal(description, returnedCategory.Description);

            // Verify the ID was set correctly in the DTO
            _mockCategoryRepository.Verify(repo => repo.UpdateCategoryAsync(It.Is<CategoryDTO>(dto => 
                dto.Id == categoryId && 
                dto.Name == name && 
                dto.Description == description)), 
                Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(999)]
        public async Task UpdateCategory_ReturnsNotFound_WhenCategoryDoesNotExist(int categoryId)
        {
            // Arrange
            var categoryDto = new CategoryDTO { Name = "Updated Electronics", Description = "Updated Electronic items" };
            
            _mockCategoryRepository.Setup(repo => repo.UpdateCategoryAsync(It.IsAny<CategoryDTO>())).ReturnsAsync((Category)null);

            // Act
            var result = await _controller.UpdateCategory(categoryId, categoryDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Category not found or update failed", notFoundResult.Value);
        }

        #endregion

        #region DeleteCategory Tests

        [Fact]
        public async Task DeleteCategory_ReturnsOkResult_WhenCategoryIsDeleted()
        {
            // Arrange
            var categoryId = 1;
            _mockCategoryRepository.Setup(repo => repo.DeleteCategoryAsync(categoryId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteCategory(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Category deleted successfully", okResult.Value);
        }

        [Fact]
        public async Task DeleteCategory_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            var categoryId = 1;
            _mockCategoryRepository.Setup(repo => repo.DeleteCategoryAsync(categoryId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteCategory(categoryId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Category not found or delete failed", notFoundResult.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task DeleteCategory_DeletesCategory_WithCorrectId(int categoryId)
        {
            // Arrange
            _mockCategoryRepository.Setup(repo => repo.DeleteCategoryAsync(categoryId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteCategory(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Category deleted successfully", okResult.Value);
            
            // Verify the repository was called with the correct ID
            _mockCategoryRepository.Verify(repo => repo.DeleteCategoryAsync(categoryId), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(999)]
        public async Task DeleteCategory_ReturnsNotFound_WhenCategoryDoesNotExist_WithInvalidId(int categoryId)
        {
            // Arrange
            _mockCategoryRepository.Setup(repo => repo.DeleteCategoryAsync(categoryId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteCategory(categoryId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Category not found or delete failed", notFoundResult.Value);
            
            // Verify the repository was called with the correct ID
            _mockCategoryRepository.Verify(repo => repo.DeleteCategoryAsync(categoryId), Times.Once);
        }

        #endregion
    }
}