using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.DTOs;


namespace Ecommerce.BackendAPI.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IList<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<IList<Category>> GetCategoriesByParentIdAsync(int parentId);
        Task<IEnumerable<Category>> SearchCategoryByPatternAsync(string pattern);
        Task<Category?> CreateCategoryAsync(CategoryDTO category, ParentCategory parentCategory);
        Task<Category?> UpdateCategoryAsync(CategoryDTO category);
        Task<bool> DeleteCategoryAsync(int id);
        Task<bool> SaveAsync();
    }
}