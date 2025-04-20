using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.ParametersType;

namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IAdminRepository
    {
        Task<Admin?> GetAdminById(int Id);
        // Task<Customer?> UpdateCustomer(Customer customer);
        // Task<int> ChangePassword(int Id, ChangePasswordParameter request);
        // Task<IEnumerable<Customer>> GetAllCustomers();
    }
}