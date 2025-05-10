using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.DTOs;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.BackendAPI.FiltersAction;



namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentCategoryController : ControllerBase
    {
        private readonly IParentCategoryRepo _parentCategoryRepo;

        public ParentCategoryController(IParentCategoryRepo parentCategoryRepo)
        {
            _parentCategoryRepo = parentCategoryRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllParentCategories()
        {
            var parentCategories = await _parentCategoryRepo.GetAllParentCategoriesAsync();
            return Ok(parentCategories);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetParentCategoryById(int id)
        {
            var parentCategory = await _parentCategoryRepo.GetParentCategoryByIdAsync(id);
            if (parentCategory == null)
            {
                return NotFound();
            }
            return Ok(parentCategory);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchParentCategoryByPattern([FromQuery] string pattern)
        {
            var parentCategories = await _parentCategoryRepo.SearchParentCategoryByPatternAsync(pattern);
            return Ok(parentCategories);
        }

        [HttpPost]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyAdmin))]
        public async Task<IActionResult> CreateParentCategory([FromBody] ParentCategoryDTO request)
        {
            if (request == null)
            {
                return BadRequest("Parent category cannot be null.");
            }

            var parentCategories = await _parentCategoryRepo.GetAllParentCategoriesAsync();
            var checkedParentCategory = parentCategories
                .Where(pc => pc.Name.Trim().ToUpper() == request.Name.Trim().ToUpper())
                .FirstOrDefault();
            if (checkedParentCategory != null)
            {
                return BadRequest($"Parent category {request.Name} already exists.");
            }

            var createdParentCategory = await _parentCategoryRepo.CreateParentCategoryAsync(request.Name);
            if (createdParentCategory == null) return BadRequest("Can not create parent category");
            return Ok(createdParentCategory);
        }
    

        [HttpPut("{Id}")]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyAdmin))]
        [ServiceFilter(typeof(CategoryAndParentAndClassificationFilter))]
        public async Task<IActionResult> UpdateParentCategory(int Id, [FromBody] ParentCategoryDTO request)
        {
            if (request == null) return BadRequest("Invalid category data");
            request.Id = Id;
            

            var response = await _parentCategoryRepo.UpdateParentCategoryAsync(request);
            if (response == null) {
                return NotFound($"Parent category with ID {Id} not found.");
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyAdmin))]
        [ServiceFilter(typeof(CategoryAndParentAndClassificationFilter))]
        public async Task<IActionResult> DeleteParentCategory(int id)
        {
            if (!await _parentCategoryRepo.DeleteParentCategoryAsync(id))
            {
                return NotFound($"Parent category with ID {id} not found.");
            }

            return Ok($"Parent category with ID {id} deleted successfully.");
        }
    }
}