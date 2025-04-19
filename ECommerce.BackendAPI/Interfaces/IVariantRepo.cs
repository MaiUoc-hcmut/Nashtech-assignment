using Ecommerce.SharedViewModel.Models;
using Microsoft.EntityFrameworkCore.Storage;


namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IVariantRepository
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<Variant?> GetVariantById(int id);
        Task<IEnumerable<Variant>> GetVariantsByProductId(int productId);
        Task<bool> CreateVariant(Variant variant);
        Task<bool> UpdateVariant(Variant variant);
        Task<bool> DeleteVariant(int id);
        Task<bool> Save();
    }
}