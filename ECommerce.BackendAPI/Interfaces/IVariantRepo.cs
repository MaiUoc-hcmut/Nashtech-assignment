using Ecommerce.SharedViewModel.Models;
using Microsoft.EntityFrameworkCore.Storage;


namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IVariantRepository
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<Variant?> GetVariantByIdAsync(int id);
        Task<IEnumerable<Variant>> GetVariantsByProductIdAsync(int productId);
        Task<bool> CreateVariantAsync(Variant variant);
        Task<bool> UpdateVariantAsync(Variant variant);
        Task<bool> DeleteVariantAsync(int id);
        Task<bool> SaveAsync();
    }
}