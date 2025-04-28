using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.BackendAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.BackendAPI.FiltersAction;


namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassificationController : ControllerBase
    {
        private readonly IClassificationRepository _classificationRepository;

        public ClassificationController(IClassificationRepository classificationRepository)
        {
            _classificationRepository = classificationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClassifications()
        {
            var classifications = await _classificationRepository.GetAllClassifications();
            return Ok(classifications);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetClassificationById(int Id)
        {
            var classification = await _classificationRepository.GetClassificationById(Id);
            if (classification == null) {
                return NotFound(new { Error = "Classification not found" });
            }
            return Ok(classification);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchClassificationByPattern ([FromQuery] string pattern)
        {
            var response = await _classificationRepository.SearchClassificationByPattern(pattern);
            return Ok(response);
        }

        [HttpPost]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyAdmin))]
        public async Task<IActionResult> CreateClassification([FromBody] ClassificationDTO request)
        {
            if (request == null) return BadRequest(new { Error = "Name must be not null" });

            var classification = new Classification
            {
                Name = request.Name,
                Description = request.Description
            };

            var response = await _classificationRepository.CreateClassification(classification);
            return Ok(response);
        }

        [HttpPut("{Id}")]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyAdmin))]
        [ServiceFilter(typeof(CategoryAndParentAndClassificationFilter))]
        public async Task<IActionResult> UpdateClassification(int Id, ClassificationDTO request)
        {
            if (request == null)   return BadRequest("Invalid category data");
            request.Id = Id;

            var response = await _classificationRepository.UpdateClassification(request);
            if (response == null) return NotFound(new { Error = "Classification not found" });
            return Ok(response);
        }

        [HttpDelete("{Id}")]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyAdmin))]
        [ServiceFilter(typeof(CategoryAndParentAndClassificationFilter))]
        public async Task<IActionResult> DeleteClassification(int Id)
        {
            var classification = HttpContext.Items["Classification"] as Classification;
            if (classification == null) return BadRequest("Bad request");

            if (!await _classificationRepository.DeleteClassification(classification))
            {
                return NotFound(new { Error = "Classification not found" });
            }

            return Ok(new { Message = "Classification successfully deleted"});
        }
    }
}