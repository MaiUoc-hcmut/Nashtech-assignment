using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.ParametersType;
using Ecommerce.BackendAPI.Data;
using Ecommerce.BackendAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BackendAPI.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> Register(RegisterParameter request)
        {
            // Check if the username already exists
            var existingCustomer = await _context.Customers.FindAsync(request.Username);
            if (existingCustomer != null)
            {
                return false; // Username already exists
            }

            // Create a new customer object
            var customer = new Customer
            {
                Name = request.Name,
                Username = request.Username,
                Password = request.Password, 
                Address = request.Address,
                PhoneNumber = request.PhoneNumber
            };

            // Add the customer to the database
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Customer?> Login(LoginParameter request)
        {
            // Check if the customer exists with the provided username and password
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Username == request.Username && c.Password == request.Password);

            return customer; // Return the customer if found, otherwise null
        }
    }
}