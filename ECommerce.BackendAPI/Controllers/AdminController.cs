using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.BackendAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.BackendAPI.FiltersAction;

namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly AuthService _authService;

        public AdminController(IAdminRepository adminRepository, AuthService authService)
        {
            _adminRepository = adminRepository;
            _authService = authService;
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

        [HttpPost]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyAdmin))]
        public async Task<IActionResult> CreateAdminAccount(AdminDTO request) 
        {
            var admin = new Admin
            {
                Name = request.Name,
                Email = request.Email,
                Password = _authService.HashPassword(request.Password),
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
            };

            var response = await _adminRepository.CreateAdminAccount(admin);
            return Ok();
        }
    }
}