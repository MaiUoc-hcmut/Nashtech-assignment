using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.Models;
using Microsoft.AspNetCore.Mvc;



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
            var parentCategories = await _parentCategoryRepo.GetAllParentCategories();
            return Ok(parentCategories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetParentCategoryById(int id)
        {
            var parentCategory = await _parentCategoryRepo.GetParentCategoryById(id);
            if (parentCategory == null)
            {
                return NotFound();
            }
            return Ok(parentCategory);
        }

        [HttpPost]
        public async Task<IActionResult> CreateParentCategory([FromBody] string request)
        {
            if (request == null)
            {
                return BadRequest("Parent category cannot be null.");
            }

            var parentCategories = await _parentCategoryRepo.GetAllParentCategories();
            var checkedParentCategory = parentCategories
                .Where(pc => pc.Name.Trim().ToUpper() == request.Name.Trim().ToUpper())
                .FirstOrDefault();
            if (checkedParentCategory != null)
            {
                return BadRequest($"Parent category {request.Name} already exists.");
            }

            var createdParentCategory = await _parentCategoryRepo.CreateParentCategory(request.Name);
            return Ok(createdParentCategory);
        }
    }
}