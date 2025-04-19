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
        public DbSet<Variant> Variants { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ParentCategory> ParentCategories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Review> Reviews { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VariantCategory>()
                .HasKey(vc => new { vc.VariantId, vc.CategoryId });
            modelBuilder.Entity<VariantCategory>()
                .HasOne(v => v.Variant)
                .WithMany(vc => vc.VariantCategories)
                .HasForeignKey(vc => vc.VariantId);
            modelBuilder.Entity<VariantCategory>()
                .HasOne(c => c.Category)
                .WithMany(vc => vc.VariantCategories)
                .HasForeignKey(vc => vc.CategoryId);

            modelBuilder.Entity<VariantOrder>()
                .HasKey(vo => new { vo.VariantId, vo.OrderId });
            modelBuilder.Entity<VariantOrder>()
                .HasOne(v => v.Variant)
                .WithMany(vo => vo.VariantOrders)
                .HasForeignKey(vo => vo.VariantId);
            modelBuilder.Entity<VariantOrder>()
                .HasOne(o => o.Order)
                .WithMany(vo => vo.VariantOrders)
                .HasForeignKey(vo => vo.OrderId);

            modelBuilder.Entity<VariantCart>()
                .HasKey(vc => new { vc.VariantId, vc.CartId });
            modelBuilder.Entity<VariantCart>()
                .HasOne(v => v.Variant)
                .WithMany(vc => vc.VariantCarts)
                .HasForeignKey(vc => vc.VariantId);
            modelBuilder.Entity<VariantCart>()
                .HasOne(c => c.Cart)
                .WithMany(vc => vc.VariantCarts)
                .HasForeignKey(vc => vc.CartId);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Customer)
                .WithOne(cu => cu.cart)
                .HasForeignKey<Cart>(c => c.Id);


            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
            
            modelBuilder.Entity<Variant>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);
        }   
    }
}