using Ecommerce.SharedViewModel.ParametersType;
using Ecommerce.SharedViewModel.Responses;
using Ecommerce.SharedViewModel.DTOs;

namespace Ecommerce.ClientMVC.Interface
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginParameter model);
        Task<RegisterResponse> RegisterAsync(RegisterParameter model);
        void StoreUserData(LoginResponse response);
        CustomerResponse? GetCurrentUser();
        Task<bool> IsAuthenticatedAsync();
        Task<bool> LogoutAsync();
    }
}