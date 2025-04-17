using Ecommerce.SharedViewModel.Models;


namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IVariantRepository
    {
        Task<Variant?> GetVariantById(int id);
        Task<IEnumerable<Variant>> GetVariantsByProductId(int productId);
        Task<bool> AddVariant(Variant variant);
        Task<bool> UpdateVariant(Variant variant);
        Task<bool> DeleteVariant(int id);
        Task<bool> Save();
    }
}