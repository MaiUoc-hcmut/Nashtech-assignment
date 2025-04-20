using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.BackendAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.SharedViewModel.Models;
using AutoMapper;
using Ecommerce.BackendAPI.FiltersAction;


namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IParentCategoryRepo _parentCategoryRepo;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IParentCategoryRepo parentCategoryRepo, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _parentCategoryRepo = parentCategoryRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = _mapper.Map<IList<CategoryDTO>>(await _categoryRepository.GetAllCategories());
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = _mapper.Map<CategoryDTO>(await _categoryRepository.GetCategoryById(id));
            if (category == null) return NotFound("Category not found");
            return Ok(category);
        }

        [HttpGet("GetCategoriesByParentId/{parentId}")]
        public async Task<IActionResult> GetCategoriesByParentId(int parentId)
        {
            var categories = _mapper.Map<IList<CategoryDTO>>(await _categoryRepository.GetCategoriesByParentId(parentId));
            if (categories == null || categories.Count == 0) return NotFound("No categories found for this parent ID");
            return Ok(categories);
        }

        [HttpPost]
        [ServiceFilter(typeof(CategoryAndParentFilter))]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO categoryDto, [FromQuery] int parentId)
        {
            if (categoryDto == null) return BadRequest("Invalid category data");

            var parentCategory = await _parentCategoryRepo.GetParentCategoryById(parentId);
            if (parentCategory == null) return NotFound("Parent category not found");

            await _categoryRepository.CreateCategory(categoryDto, parentCategory);
            return Ok("Category created successfully");
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(CategoryAndParentFilter))]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDTO categoryDto)
        {
            if (categoryDto == null || id != categoryDto.Id) return BadRequest("Invalid category data");

            var updated = await _categoryRepository.UpdateCategory(categoryDto);
            if (!updated) return NotFound("Category not found or update failed");

            return Ok("Category updated successfully");
        }
        
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(CategoryAndParentFilter))]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var deleted = await _categoryRepository.DeleteCategory(id);
            if (!deleted) return NotFound("Category not found or delete failed");

            return Ok("Category deleted successfully");
        }
    }
}