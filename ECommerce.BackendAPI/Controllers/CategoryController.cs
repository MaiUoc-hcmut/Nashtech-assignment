using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.BackendAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.BackendAPI.FiltersAction;


namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IParentCategoryRepo _parentCategoryRepo;

        public CategoryController(ICategoryRepository categoryRepository, IParentCategoryRepo parentCategoryRepo)
        {
            _categoryRepository = categoryRepository;
            _parentCategoryRepo = parentCategoryRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetCategoryById(int Id)
        {
            var category = await _categoryRepository.GetCategoryById(Id);
            if (category == null) return NotFound("Category not found");
            return Ok(category);
        }

        [HttpGet("GetCategoriesByParentId/{parentId}")]
        public async Task<IActionResult> GetCategoriesByParentId(int parentId)
        {
            var categories = await _categoryRepository.GetCategoriesByParentId(parentId);
            if (categories == null || categories.Count == 0) return NotFound("No categories found for this parent ID");
            return Ok(categories);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCategoryByPattern([FromQuery] string pattern)
        {
            var categories = await _categoryRepository.SearchCategoryByPattern(pattern);
            return Ok(categories);
        }

        [HttpPost]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyAdmin))]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO categoryDto, [FromQuery] int parentId)
        {
            if (categoryDto == null) return BadRequest("Invalid category data");

            var parentCategory = await _parentCategoryRepo.GetParentCategoryById(parentId);
            if (parentCategory == null) return NotFound("Parent category not found");

            var categoryCreated = await _categoryRepository.CreateCategory(categoryDto, parentCategory);
            return Ok(categoryCreated);
        }

        [HttpPut("{Id}")]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyAdmin))]   
        [ServiceFilter(typeof(CategoryAndParentAndClassificationFilter))]     
        public async Task<IActionResult> UpdateCategory(int Id, [FromBody] CategoryDTO request)
        {
            if (request == null) return BadRequest("Invalid category data");
            request.Id = Id;

            var categoryUpdated = await _categoryRepository.UpdateCategory(request);
            if (categoryUpdated == null) return NotFound("Category not found or update failed");

            return Ok(categoryUpdated);
        }
        
        [HttpDelete("{Id}")]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyAdmin))]
        [ServiceFilter(typeof(CategoryAndParentAndClassificationFilter))]
        public async Task<IActionResult> DeleteCategory(int Id)
        {
            var deleted = await _categoryRepository.DeleteCategory(Id);
            if (!deleted) return NotFound("Category not found or delete failed");

            return Ok("Category deleted successfully");
        }
    }
}