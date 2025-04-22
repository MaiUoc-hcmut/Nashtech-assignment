using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Models;
using Microsoft.EntityFrameworkCore;
using Ecommerce.BackendAPI.Data;
using Ecommerce.BackendAPI.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;


namespace Ecommerce.BackendAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllProducts
        (
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "UpdatedAt",
            bool isAsc = true
        )
        {
            var query = _context.Products.AsQueryable();

            query = isAsc
                ? query.OrderBy(p => EF.Property<object>(p, sortBy))
                : query.OrderByDescending(p => EF.Property<object>(p, sortBy));

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return await query.ToListAsync();
        }

        public async Task<Product> CreateProduct(Product product)
        {
            var response = await _context.Products.AddAsync(product);
            await Save();
            return response.Entity;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            return await Save();
        }
        
        public async Task<bool> DeleteProduct(int id)
        {
            var product = await GetProductById(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}