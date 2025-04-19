using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.Models;
using Microsoft.EntityFrameworkCore;
using Ecommerce.BackendAPI.Data;
using Microsoft.EntityFrameworkCore.Storage;


namespace Ecommerce.BackendAPI.Repositories
{
    public class VariantRepository : IVariantRepository
    {
        private readonly DataContext _context;

        public VariantRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<Variant?> GetVariantById(int id)
        {
            return await _context.Variants.FindAsync(id);
        }

        public async Task<IEnumerable<Variant>> GetVariantsByProductId(int productId)
        {
            return await _context.Variants.Where(v => v.Product.Id == productId).ToListAsync();
        }

        public async Task<bool> CreateVariant(Variant variant)
        {
            await _context.Variants.AddAsync(variant);
            return await Save();
        }

        public async Task<bool> UpdateVariant(Variant variant)
        {
            _context.Variants.Update(variant);
            return await Save();
        }

        public async Task<bool> DeleteVariant(int id)
        {
            var variant = await GetVariantById(id);
            if (variant == null) return false;

            _context.Variants.Remove(variant);
            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}