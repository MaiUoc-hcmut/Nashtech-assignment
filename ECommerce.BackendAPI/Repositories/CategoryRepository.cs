using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.BackendAPI.Data;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce.BackendAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IList<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Include(c => c.ParentCategory)
                .Select(c => new Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ParentCategory = c.ParentCategory
                })
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<IList<Category>> GetCategoriesByParentIdAsync(int parentId)
        {
            return await _context.Categories.Where(c => c.ParentCategory.Id == parentId).ToListAsync();
        }

        public async Task<IEnumerable<Category>> SearchCategoryByPatternAsync(string pattern)
        {
            var query = _context.Categories.AsQueryable();
            var term = pattern.ToLower();
            query = query.Where(c => c.Name.Contains(term));

            var response = await query.ToListAsync();

            return response;
        }

        public async Task<Category?> CreateCategoryAsync(CategoryDTO request, ParentCategory parentCategory)
        {
            var category = new Category
            {
                Name = request.Name,
                Description = request.Description,
                ParentCategory = parentCategory
            };

            var response = await _context.Categories.AddAsync(category);
            return (await SaveAsync() == true) ? response.Entity : null;
        }

        public async Task<Category?> UpdateCategoryAsync(CategoryDTO request)
        {
            var category = await _context.Categories.FindAsync(request.Id);
            if (category == null) return null;

            category.Name = request.Name;
            category.Description = request.Description;
            _context.Categories.Update(category);
            return (await SaveAsync() == true) ? category : null;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}