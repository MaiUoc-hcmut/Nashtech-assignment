using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.BackendAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
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

        [HttpPost]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(CategoryAndParentAndClassificationFilter))]
        public async Task<IActionResult> CreateClassification(string Name)
        {
            if (Name == null) return BadRequest(new { Error = "Name must be not null" });

            var classification = new Classification
            {
                Name = Name
            };

            await _classificationRepository.CreateClassification(classification);
            return Ok(new { Message = "Classification successfully created" });
        }

        [HttpPut("{Id}")]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(CategoryAndParentAndClassificationFilter))]
        public async Task<IActionResult> UpdateClassification(int Id, ClassificationDTO request)
        {
            if (Id != request.Id || request == null)   return BadRequest("Invalid category data");

            if (!await _classificationRepository.UpdateClassification(request)) {
                return NotFound(new { Error = "Classification not found" });
            }

            return Ok(new { Message = "Classification successfully updated"});
        }

        [HttpDelete]
        [ServiceFilter(typeof(VerifyToken))]
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