using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.ParametersType;


namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> Register(RegisterParameter request);
        Task<Customer?> Login(LoginParameter request);
    }
}