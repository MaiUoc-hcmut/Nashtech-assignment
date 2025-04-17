using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Models;
using Microsoft.EntityFrameworkCore;
using Ecommerce.BackendAPI.Data;
using Ecommerce.BackendAPI.Interfaces;


namespace Ecommerce.BackendAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}