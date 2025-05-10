using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.ParametersType;
using Microsoft.EntityFrameworkCore.Storage;


namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IAuthRepository
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<Customer> RegisterAsync(RegisterParameter request);
        Task<Customer?> LoginAsync(LoginParameter request);
        Task<Admin?> AdminLoginAsync(LoginParameter request);
    }
}