using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.BackendAPI.Data;
using Ecommerce.SharedViewModel.ParametersType;
using Microsoft.EntityFrameworkCore;
using Ecommerce.SharedViewModel.DTOs;


namespace Ecommerce.BackendAPI.Repositories
{
    public class ParentCategoryRepository : IParentCategoryRepo
    {
        private readonly DataContext _context;

        public ParentCategoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<ParentCategory>> GetAllParentCategoriesAsync()
        {
            return await _context.ParentCategories.AsNoTracking().ToListAsync();
        }

        public async Task<ParentCategory?> GetParentCategoryByIdAsync(int id)
        {
            return await _context.ParentCategories.FindAsync(id);
        }

        public async Task<ParentCategory?> GetParentCategoryByNameAsync(string name)
        {
            return await _context.ParentCategories.FirstOrDefaultAsync(pc => pc.Name == name);
        }

        public async Task<List<ParentCategory>> SearchParentCategoryByPatternAsync(string pattern)
        {
            var query = _context.ParentCategories.AsQueryable();
            var term = pattern.ToLower();
            query = query.Where(c => c.Name.Contains(term));

            var response = await query.ToListAsync();

            return response;
        }

        public async Task<ParentCategory?> CreateParentCategoryAsync(string Name)
        {
            var parentCategory = await _context.ParentCategories.AddAsync(new ParentCategory { Name = Name });
            return (await SaveAsync() == true) ? parentCategory.Entity : null;
        }

        public async Task<ParentCategory?> UpdateParentCategoryAsync(ParentCategoryDTO request)
        {
            var parentCategory = await _context.ParentCategories.FindAsync(request.Id);
            if (parentCategory == null) return null;

            parentCategory.Name = request.Name;
            parentCategory.Description = request.Description;
            _context.ParentCategories.Update(parentCategory);
            return (await SaveAsync() == true) ? parentCategory : null;
        }

        public async Task<bool> DeleteParentCategoryAsync(int id)
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