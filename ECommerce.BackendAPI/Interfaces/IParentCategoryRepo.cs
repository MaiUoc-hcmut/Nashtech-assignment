using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Models;

namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IParentCategoryRepo
    {
        Task<List<ParentCategory>> GetAllParentCategoriesAsync();
        Task<ParentCategory?> GetParentCategoryByIdAsync(int id);
        Task<ParentCategory?> GetParentCategoryByNameAsync(string name);
        Task<List<ParentCategory>> SearchParentCategoryByPatternAsync(string pattern);
        Task<ParentCategory?> CreateParentCategoryAsync(string Name);
        Task<ParentCategory?> UpdateParentCategoryAsync(ParentCategoryDTO parentCategory);
        Task<bool> DeleteParentCategoryAsync(int id);
        Task<bool> SaveAsync();
    }
} 