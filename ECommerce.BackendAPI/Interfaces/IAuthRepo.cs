using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.ParametersType;
using Microsoft.EntityFrameworkCore.Storage;


namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IAuthRepository
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<Customer> Register(RegisterParameter request);
        Task<Customer?> Login(LoginParameter request);
        Task<Admin?> AdminLogin(LoginParameter request);
    }
}