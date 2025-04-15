using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.ParametersType;
using Ecommerce.BackendAPI.Data;
using Ecommerce.BackendAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ecommerce.BackendAPI.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<Customer> Register(RegisterParameter request)
        {
            
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

            return customer;
        }

        public async Task<Customer?> Login(LoginParameter request)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Username == request.Username);
            if (customer == null) return null;
            bool isValid = BCrypt.Net.BCrypt.Verify(request.Password, customer.Password);
            return isValid ? customer : null;
        }
    }
}