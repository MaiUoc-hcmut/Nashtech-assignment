using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Models;

namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IParentCategoryRepo
    {
        Task<List<ParentCategory>> GetAllParentCategories();
        Task<ParentCategory?> GetParentCategoryById(int id);
        Task<ParentCategory?> GetParentCategoryByName(string name);
        Task<List<ParentCategory>> SearchParentCategoryByPattern(string pattern);
        Task<ParentCategory?> CreateParentCategory(string Name);
        Task<ParentCategory?> UpdateParentCategory(ParentCategoryDTO parentCategory);
        Task<bool> DeleteParentCategory(int id);
        Task<bool> SaveAsync();
    }
} 