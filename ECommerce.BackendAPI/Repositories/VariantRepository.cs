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
            return await _context.Variants
                .Include(v => v.VariantCategories)
                .ThenInclude(vc => vc.Category)
                .Where(v => v.Product.Id == productId)
                .Select(v => new Variant
                {
                    Id = v.Id,
                    Price = v.Price,
                    StockQuantity = v.StockQuantity,
                    SKU = v.SKU,
                    CreatedAt = v.CreatedAt,
                    UpdatedAt = v.UpdatedAt,
                    VariantCategories = v.VariantCategories.Select(vc => new VariantCategory
                    {
                        Category = new Category
                        {
                            Id = vc.Category.Id,
                            Name = vc.Category.Name,
                            ParentCategory = vc.Category.ParentCategory,
                        }
                    }).ToList()
                })
                .ToListAsync();
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