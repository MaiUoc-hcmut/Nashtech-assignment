using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.Models;
using Microsoft.EntityFrameworkCore;
using Ecommerce.BackendAPI.Data;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.SharedViewModel.Responses;
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

        public void AttachProductClassification(ProductClassification productClassification)
        {
            _context.ProductClassifications.Attach(productClassification);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Products
                .Include(p => p.Reviews)
                .Include(p => p.Variants)
                .ThenInclude(v => v.VariantOrders)
                .Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    Variants = p.Variants,
                })
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<GetAllProductsResponse>> GetAllProducts
        (
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "UpdatedAt",
            bool isAsc = true,
            int? classificationId = null
        )
        {
            var query = _context.Products
                .Include(p => p.Reviews) 
                .Include(p => p.Variants)
                .Include(p => p.ProductClassifications) 
                .ThenInclude(pc => pc.Classification) 
                .AsQueryable();

            if (classificationId.HasValue)
            {
                query = query.Where(p => p.ProductClassifications.Any(pc => pc.ClassificationId == classificationId.Value));
            }

            query = isAsc
                ? query.OrderBy(p => EF.Property<object>(p, sortBy))
                : query.OrderByDescending(p => EF.Property<object>(p, sortBy));

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var productsWithRatings = await query
                .Select(p => new GetAllProductsResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0,
                    TotalOrders = p.Variants.SelectMany(vo => vo.VariantOrders).Count()
                })
                .ToListAsync();

            return productsWithRatings;
        }

        public async Task<Product> CreateProduct(Product product, IList<Classification> classificationList)
        {
            var response = await _context.Products.AddAsync(product);
            foreach (var classification in classificationList) {
                var newItems = new ProductClassification
                {
                    Product = response.Entity,
                    Classification = classification
                };
                await _context.ProductClassifications.AddAsync(newItems);
            }
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