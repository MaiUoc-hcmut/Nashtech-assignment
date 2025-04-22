using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.BackendAPI.Data;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce.BackendAPI.Repositories
{
    public class ClassificationRepository : IClassificationRepository
    {
        private readonly DataContext _context;
        
        public ClassificationRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Classification>> GetAllClassifications()
        {
            return await _context.Classifications.ToListAsync();
        }

        public async Task<Classification?> GetClassificationById(int Id)
        {
            return await _context.Classifications.FindAsync(Id);
        }

        public async Task<bool> CreateClassification(Classification classification)
        {
            await _context.Classifications.AddAsync(classification);
            return await SaveAsync();
        }

        public async Task<bool> UpdateClassification(ClassificationDTO request)
        {
            var classification = await _context.Classifications.FindAsync(request.Id);
            if (classification == null) return false;

            classification.Name = request.Name;
            _context.Classifications.Update(classification);
            return await SaveAsync();
        }

        public async Task<bool> DeleteClassification(Classification classification)
        {
            _context.Classifications.Remove(classification);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}