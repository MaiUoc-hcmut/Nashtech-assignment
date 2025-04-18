using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ecommerce.BackendAPI.FiltersAction;
using Ecommerce.SharedViewModel.ParametersType;


namespace Ecommerce.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = _mapper.Map<CustomerDTO>(await _customerRepository.GetCustomerById(id));
            if (customer == null)
            {
                return NotFound(new { Message = "Customer not found." });
            }
            return Ok(customer);
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
            
            var customer = _mapper.Map<Customer>(customerDTO);
            customer.Id = userId;
            var updatedCustomer = await _customerRepository.UpdateCustomer(customer);
            if (updatedCustomer == null)
            {
                return NotFound(new { Message = "Customer not found." });
            }

            return Ok(_mapper.Map<CustomerDTO>(updatedCustomer));
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