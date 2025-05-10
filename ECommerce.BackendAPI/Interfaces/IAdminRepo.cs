using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.ParametersType;

namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IAdminRepository
    {
        Task<Admin?> GetAdminByIdAsync(int Id);
        Task<Admin?> CreateAdminAccountAsync(Admin admin);
        // Task<int> ChangePassword(int Id, ChangePasswordParameter request);
        // Task<IEnumerable<Customer>> GetAllCustomers();
        Task<bool> SaveAsync();
    }
}