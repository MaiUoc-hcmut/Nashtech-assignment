using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.ParametersType;

namespace Ecommerce.BackendAPI.Interfaces
{
    public interface ICustomerRepository
    {
        Task<(int TotalCustomers, IEnumerable<Customer> Customers)> GetCustomers(int pageNumber = 1);
        Task<Customer?> GetCustomerById(int Id);
        Task<Customer?> GetCustomerByUsername(string username);
        Task<Customer?> GetCustomerByEmail(string email);
        Task<Customer?> UpdateCustomer(UpdateCustomerParameter customer);
        Task<int> ChangePassword(int Id, ChangePasswordParameter request);
        // Task<IEnumerable<Customer>> GetAllCustomers();
    }
}