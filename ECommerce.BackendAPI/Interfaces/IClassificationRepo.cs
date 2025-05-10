using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Models;


namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IClassificationRepository
    {
        Task<IEnumerable<Classification>> GetAllClassificationsAsync();
        Task<Classification?> GetClassificationByIdAsync(int Id);
        Task<IEnumerable<Classification>> SearchClassificationByPatternAsync(string pattern);
        Task<Classification?> CreateClassificationAsync(Classification classification);
        Task<Classification?> UpdateClassificationAsync(ClassificationDTO request);
        Task<bool> DeleteClassificationAsync(Classification classification);
        Task<bool> SaveAsync();
    }
}