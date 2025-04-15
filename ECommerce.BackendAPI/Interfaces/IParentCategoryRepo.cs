using Ecommerce.SharedViewModel.Models;

namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IParentCategoryRepo
    {
        Task<List<ParentCategory>> GetAllParentCategories();
        Task<ParentCategory?> GetParentCategoryById(int id);
        Task<ParentCategory?> GetParentCategoryByName(string name);
        Task<ParentCategory> CreateParentCategory(string name);
        Task<ParentCategory> UpdateParentCategory(ParentCategory parentCategory);
        Task<bool> DeleteParentCategory(int id);
    }
} 