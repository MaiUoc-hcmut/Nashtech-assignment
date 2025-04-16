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

        public async Task<IList<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<IList<Category>> GetCategoriesByParentId(int parentId)
        {
            return await _context.Categories.Where(c => c.ParentCategory.Id == parentId).ToListAsync();
        }

        public async Task<Category> CreateCategory(CategoryDTO request, ParentCategory parentCategory)
        {
            var category = new Category
            {
                Name = request.Name,
                ParentCategory = parentCategory
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> UpdateCategory(CategoryDTO request)
        {
            var category = await _context.Categories.FindAsync(request.Id);
            if (category == null) return false;

            category.Name = request.Name;
            _context.Categories.Update(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}