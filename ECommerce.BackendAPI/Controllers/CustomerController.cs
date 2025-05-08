using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Models;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.BackendAPI.FiltersAction;
using Ecommerce.SharedViewModel.ParametersType;


namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        [ServiceFilter(typeof(VerifyToken))]
        [ServiceFilter(typeof(VerifyAdmin))]
        public async Task<IActionResult> GetCustomers([FromQuery] int pageNumber = 1)
        {
            try
            {
                var (totalCustomers, customers) = await _customerRepository.GetCustomers(pageNumber);

                // Return the response
                return Ok(new
                {
                    TotalCustomers = totalCustomers,
                    Customers = customers.Select(c => new
                    {
                        c.Id,
                        c.Name,
                        c.Email,
                        c.Username,
                        c.PhoneNumber,
                        c.Address,
                        c.CreatedAt
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving customers.", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await _customerRepository.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound(new { Message = "Customer not found." });
            }
            return Ok(new {
                Name = customer.Name,
                Email = customer.Email,
                UserName = customer.Username,
                Address = customer.Address,
                PhoneNumber = customer.PhoneNumber
            });
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(CheckUserExists))]
        [ServiceFilter(typeof(VerifyToken))]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDTO customerDTO)
        {

            string? rawId = HttpContext.Items["UserId"] as string;
            int userId = int.Parse(rawId ?? "0");
            if (userId != id)
            {
                return Unauthorized(new { Message = "You are not authorized to update this customer." });
            }
            
            var customer = new UpdateCustomerParameter 
            {
                Id = userId,
                Name = customerDTO.Name,
                Email = customerDTO.Email,
                Username = customerDTO.Username,
                PhoneNumber = customerDTO.PhoneNumber,
                Address = customerDTO.Address
            };
            customer.Id = userId;
            var updatedCustomer = await _customerRepository.UpdateCustomer(customer);
            if (updatedCustomer == null)
            {
                return NotFound(new { Message = "Customer not found." });
            }

            return Ok(new {
                Id = updatedCustomer.Id,
                Name = updatedCustomer.Name,
                Email = updatedCustomer.Email,
                Username = updatedCustomer.Username,
                PhoneNumber = updatedCustomer.PhoneNumber,
                Address = updatedCustomer.Address
            });
        }
    

        [HttpPut("ChangePassword/{id}")]
        [ServiceFilter(typeof(CheckUserExists))]
        [ServiceFilter(typeof(VerifyToken))]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordParameter request)
        {
            string? rawId = HttpContext.Items["UserId"] as string;
            int userId = int.Parse(rawId ?? "0");
            if (userId != id)
            {
                return Unauthorized(new { Message = "You are not authorized to change this customer's password." });
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                return BadRequest(new { Message = "New password and confirmation do not match." });
            }

            var result = await _customerRepository.ChangePassword(userId, request);
            if (result == 1)
            {
                return NotFound(new { Message = "Customer not found." });
            }
            else if (result == 2)
            {
                return BadRequest(new { Message = "Old password is incorrect." });
            }

            return Ok(new { Message = "Password changed successfully." });
        }
    }
}