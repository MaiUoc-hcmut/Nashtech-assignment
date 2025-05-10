using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.BackendAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.BackendAPI.FiltersAction;
using Ecommerce.SharedViewModel.ParametersType;
using Ecommerce.BackendAPI.Data;

namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IAuthService _authService;
        private readonly DataContext _context;

        public AdminController(IAdminRepository adminRepository, IAuthService authService,DataContext context)
        {
            _adminRepository = adminRepository;
            _authService = authService;
            _context = context;
        }

        [HttpGet]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyAdmin))]
        public IActionResult GetSelfInfo()
        {
            var admin = HttpContext.Items["admin"] as Admin;
            if (admin == null)
            {
                return NotFound(new { Message = "Admin not found" });
            }

            return Ok(admin);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdminById(int id)
        {
            var admin = await _adminRepository.GetAdminByIdAsync(id);
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

            var response = await _adminRepository.CreateAdminAccountAsync(admin);
            return Ok(response);
        }
    
        [HttpPost("changePassword")]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyAdmin))]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordParameter request)
        {
            var admin = HttpContext.Items["admin"] as Admin;
            if (admin == null) 
            {
                return NotFound("Admin not found");
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                return BadRequest("New password and password does not match");
            }

            if (!_authService.VerifyPassword(request.OldPassword, admin.Password))
            {
                return BadRequest("New password does not match with current password");
            }

            admin.Password = _authService.HashPassword(request.NewPassword);
            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Password change successfully" });
        }
    }
}