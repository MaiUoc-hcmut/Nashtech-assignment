using Ecommerce.BackendAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdminById(int id)
        {
            var admin = await _adminRepository.GetAdminById(id);
            if (admin == null)
            {
                return NotFound(new { Message = "Customer not found" });
            }
            return Ok(admin);
        }
    }
}