using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.DTOs;


namespace Ecommerce.BackendAPI.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IList<Category>> GetAllCategories();
        Task<Category?> GetCategoryById(int id);
        Task<IList<Category>> GetCategoriesByParentId(int parentId);
        Task<Category> CreateCategory(CategoryDTO category, ParentCategory parentCategory);
        Task<bool> UpdateCategory(CategoryDTO category);
        Task<bool> DeleteCategory(int id);
        Task<bool> Save();
    }
}