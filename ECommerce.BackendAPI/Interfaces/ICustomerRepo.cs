using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.ParametersType;

namespace Ecommerce.BackendAPI.Interfaces
{
    public interface ICustomerRepository
    {
        Task<(int TotalCustomers, IEnumerable<Customer> Customers)> GetCustomersAsync(int pageNumber = 1);
        Task<Customer?> GetCustomerByIdAsync(int Id);
        Task<Customer?> GetCustomerByUsernameAsync(string username);
        Task<Customer?> GetCustomerByEmailAsync(string email);
        Task<Customer?> UpdateCustomerAsync(UpdateCustomerParameter customer);
        Task<int> ChangePasswordAsync(int Id, ChangePasswordParameter request);
        // Task<IEnumerable<Customer>> GetAllCustomers();
    }
}