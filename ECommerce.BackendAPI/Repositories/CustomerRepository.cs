using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.ParametersType;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.BackendAPI.Data;
using Microsoft.EntityFrameworkCore;
using Ecommerce.BackendAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;


namespace Ecommerce.BackendAPI.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;

        public CustomerRepository(DataContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<(int TotalCustomers, IEnumerable<Customer> Customers)> GetCustomers(int pageNumber = 1)
        {
            int pageSize = 10;

            // Get the total number of customers
            int totalCustomers = await _context.Customers.CountAsync();

            // Get the paginated list of customers
            var customers = await _context.Customers
                .AsNoTracking()
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Return both total customers and the paginated list
            return (totalCustomers, customers);
        }

        public async Task<Customer?> GetCustomerById(int Id)
        {
            return await _context.Customers.FindAsync(Id);
        }

        public async Task<Customer?> GetCustomerByUsername(string username)
        {
            return await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Username == username);
        }

        public async Task<Customer?> GetCustomerByEmail(string email)
        {
            return await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Customer?> UpdateCustomer(UpdateCustomerParameter customer)
        {
            var existingCustomer = await _context.Customers.FindAsync(customer.Id);
            if (existingCustomer == null) return null;

            existingCustomer.Username = customer.Username;
            existingCustomer.Email = customer.Email;
            existingCustomer.Name = customer.Name;
            existingCustomer.Address = customer.Address;
            existingCustomer.PhoneNumber = customer.PhoneNumber;

            _context.Customers.Update(existingCustomer);
            await _context.SaveChangesAsync();

            return existingCustomer;
        }

        public async Task<int> ChangePassword(int Id, ChangePasswordParameter request)
        {
            var customer = await _context.Customers.FindAsync(Id);
            if (customer == null) return 1;

            if (!_authService.VerifyPassword(request.OldPassword, customer.Password)) return 2;

            customer.Password = _authService.HashPassword(request.NewPassword);
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            return 0;
        }

    }
}
