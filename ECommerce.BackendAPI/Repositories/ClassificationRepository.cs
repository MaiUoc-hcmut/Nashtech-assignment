using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.BackendAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;


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

        public async Task<IEnumerable<Classification>> SearchClassificationByPattern(string pattern)
        {
            var query = _context.Classifications.AsQueryable();
            var term = pattern.ToLower();
            query = query.Where(c => c.Name.Contains(term));

            var response = await query.ToListAsync();

            return response;
        }

        public async Task<Classification?> CreateClassification(Classification classification)
        {
            var response = await _context.Classifications.AddAsync(classification);
            return (await SaveAsync() == true) ? response.Entity : null;
        }

        public async Task<Classification?> UpdateClassification(ClassificationDTO request)
        {
            var classification = await _context.Classifications.FindAsync(request.Id);
            if (classification == null) return null;

            classification.Name = request.Name;
            classification.Description = request.Description;
            _context.Classifications.Update(classification);
            
            return (await SaveAsync() == true) ? classification : null;
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