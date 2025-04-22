using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Models;


namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IClassificationRepository
    {
        Task<IEnumerable<Classification>> GetAllClassifications();
        Task<Classification?> GetClassificationById(int Id);
        Task<bool> CreateClassification(Classification classification);
        Task<bool> UpdateClassification(ClassificationDTO request);
        Task<bool> DeleteClassification(Classification classification);
        Task<bool> SaveAsync();
    }
}