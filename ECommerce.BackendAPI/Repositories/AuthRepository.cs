using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.ParametersType;
using Ecommerce.BackendAPI.Data;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.BackendAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ecommerce.BackendAPI.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly AuthService _authService;
        public AuthRepository(DataContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
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
                Email = request.Email,
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
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Email == request.Email);
            if (customer == null) return null;
            bool isValid = BCrypt.Net.BCrypt.Verify(request.Password, customer.Password);
            if (isValid)
            {
                customer.RefreshToken = _authService.GenerateRefreshToken(customer);
                _context.Customers.Update(customer);
                return customer;
            }
            return null;
        }
    
        public async Task<Admin?> AdminLogin(LoginParameter request)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(x => x.Email == request.Email);
            if (admin == null) return null;
            bool isValid = BCrypt.Net.BCrypt.Verify(request.Password, admin.Password);
            if (isValid)
            {
                admin.RefreshToken = _authService.GenerateRefreshToken(admin);
                _context.Admins.Update(admin);
                return admin;
            }
            return null;
        }
    }
}