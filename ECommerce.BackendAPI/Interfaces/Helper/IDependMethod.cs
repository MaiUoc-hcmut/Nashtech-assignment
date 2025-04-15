using Ecommerce.SharedViewModel.Models;


namespace Ecommerce.BackendAPI.Interfaces.Helper
{
    public interface IDependMethod
    {
        Task<Cart> CreateCartWhenRegister(Customer customer);
    }
}