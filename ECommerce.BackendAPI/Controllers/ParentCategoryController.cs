using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.ParametersType;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;



namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentCategoryController : ControllerBase
    {
        private readonly IParentCategoryRepo _parentCategoryRepo;
        private readonly IMapper _mapper;

        public ParentCategoryController(IParentCategoryRepo parentCategoryRepo, IMapper mapper)
        {
            _mapper = mapper;
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
        public async Task<IActionResult> CreateParentCategory([FromBody] ParentCategoryDTO request)
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

            var response = new ParentCategoryDTO
            {
                Id = createdParentCategory.Id,
                Name = createdParentCategory.Name
            };
            return Ok(response);
        }
    

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateParentCategory(int id, [FromBody] ParentCategoryDTO request)
        {
            var parentCategory = _mapper.Map<ParentCategory>(request);
            parentCategory.Id = id;

            if (!await _parentCategoryRepo.UpdateParentCategory(parentCategory)) 
            {
                return NotFound($"Parent category with ID {id} not found.");
            }

            return Ok($"Parent category {request.Name} updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParentCategory(int id)
        {
            if (!await _parentCategoryRepo.DeleteParentCategory(id))
            {
                return NotFound($"Parent category with ID {id} not found.");
            }

            return Ok($"Parent category with ID {id} deleted successfully.");
        }
    }
}