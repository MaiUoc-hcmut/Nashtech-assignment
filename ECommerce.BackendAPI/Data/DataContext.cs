using Ecommerce.SharedViewModel.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BackendAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ParentCategory> ParentCategories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Review> Reviews { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>()
                .HasKey(pc => new { pc.ProductId, pc.CategoryId });
            modelBuilder.Entity<ProductCategory>()
                .HasOne(p => p.Product)
                .WithMany(pc => pc.ProductCategories)
                .HasForeignKey(pc => pc.ProductId);
            modelBuilder.Entity<ProductCategory>()
                .HasOne(c => c.Category)
                .WithMany(pc => pc.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId);

            modelBuilder.Entity<ProductOrder>()
                .HasKey(po => new { po.ProductId, po.OrderId });
            modelBuilder.Entity<ProductOrder>()
                .HasOne(p => p.Product)
                .WithMany(po => po.ProductOrders)
                .HasForeignKey(po => po.ProductId);
            modelBuilder.Entity<ProductOrder>()
                .HasOne(o => o.Order)
                .WithMany(po => po.ProductOrders)
                .HasForeignKey(po => po.OrderId);

            modelBuilder.Entity<ProductCart>()
                .HasKey(pc => new { pc.ProductId, pc.CartId });
            modelBuilder.Entity<ProductCart>()
                .HasOne(p => p.Product)
                .WithMany(pc => pc.ProductCarts)
                .HasForeignKey(pc => pc.ProductId);
            modelBuilder.Entity<ProductCart>()
                .HasOne(c => c.Cart)
                .WithMany(pc => pc.ProductCarts)
                .HasForeignKey(pc => pc.CartId);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Customer)
                .WithOne(cu => cu.cart)
                .HasForeignKey<Cart>(c => c.Id);
        }   
    }
}