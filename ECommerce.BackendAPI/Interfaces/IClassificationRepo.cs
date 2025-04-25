using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Models;


namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IClassificationRepository
    {
        Task<IEnumerable<Classification>> GetAllClassifications();
        Task<Classification?> GetClassificationById(int Id);
        Task<Classification?> CreateClassification(Classification classification);
        Task<Classification?> UpdateClassification(ClassificationDTO request);
        Task<bool> DeleteClassification(Classification classification);
        Task<bool> SaveAsync();
    }
}