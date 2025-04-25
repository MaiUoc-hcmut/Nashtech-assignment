using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.BackendAPI.Data;
using Ecommerce.SharedViewModel.ParametersType;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce.BackendAPI.Repositories
{
    public class ParentCategoryRepository : IParentCategoryRepo
    {
        private readonly DataContext _context;

        public ParentCategoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<ParentCategory>> GetAllParentCategories()
        {
            return await _context.ParentCategories.AsNoTracking().ToListAsync();
        }

        public async Task<ParentCategory?> GetParentCategoryById(int id)
        {
            return await _context.ParentCategories.FindAsync(id);
        }

        public async Task<ParentCategory?> GetParentCategoryByName(string name)
        {
            return await _context.ParentCategories.FirstOrDefaultAsync(pc => pc.Name == name);
        }

        public async Task<ParentCategory?> CreateParentCategory(string Name)
        {
            var parentCategory = await _context.ParentCategories.AddAsync(new ParentCategory { Name = Name });
            return (await SaveAsync() == true) ? parentCategory.Entity : null;
        }

        public async Task<ParentCategory?> UpdateParentCategory(ParentCategory parentCategory)
        {
            var response = _context.ParentCategories.Update(parentCategory);
            return (await SaveAsync() == true) ? response.Entity : null;
        }

        public async Task<bool> DeleteParentCategory(int id)
        {
            var parentCategory = await _context.ParentCategories.FindAsync(id);
            if (parentCategory == null) return false;

            _context.ParentCategories.Remove(parentCategory);
            return await SaveAsync();
        }
    
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    
    }
}