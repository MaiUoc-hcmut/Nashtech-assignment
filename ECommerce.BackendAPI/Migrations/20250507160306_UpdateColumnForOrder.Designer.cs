﻿// <auto-generated />
using System;
using Ecommerce.BackendAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ECommerce.BackendAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250507160306_UpdateColumnForOrder")]
    partial class UpdateColumnForOrder
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Cart", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ParentCategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Classification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Classifications");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.ParentCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ParentCategories");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.ProductClassification", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("ClassificationId")
                        .HasColumnType("int");

                    b.HasKey("ProductId", "ClassificationId");

                    b.HasIndex("ClassificationId");

                    b.ToTable("ProductClassifications");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ProductId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Variant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("SKU")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StockQuantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Variants");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.VariantCart", b =>
                {
                    b.Property<int>("VariantId")
                        .HasColumnType("int");

                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("VariantId", "CartId");

                    b.HasIndex("CartId");

                    b.ToTable("VariantCarts");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.VariantCategory", b =>
                {
                    b.Property<int>("VariantId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("VariantId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("VariantCategories");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.VariantOrder", b =>
                {
                    b.Property<int>("VariantId")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("VariantId", "OrderId");

                    b.HasIndex("OrderId");

                    b.ToTable("VariantOrders");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Cart", b =>
                {
                    b.HasOne("Ecommerce.SharedViewModel.Models.Customer", "Customer")
                        .WithOne("cart")
                        .HasForeignKey("Ecommerce.SharedViewModel.Models.Cart", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Category", b =>
                {
                    b.HasOne("Ecommerce.SharedViewModel.Models.ParentCategory", "ParentCategory")
                        .WithMany("Categories")
                        .HasForeignKey("ParentCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Order", b =>
                {
                    b.HasOne("Ecommerce.SharedViewModel.Models.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.ProductClassification", b =>
                {
                    b.HasOne("Ecommerce.SharedViewModel.Models.Classification", "Classification")
                        .WithMany("ProductClassifications")
                        .HasForeignKey("ClassificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ecommerce.SharedViewModel.Models.Product", "Product")
                        .WithMany("ProductClassifications")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Classification");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Review", b =>
                {
                    b.HasOne("Ecommerce.SharedViewModel.Models.Customer", "Customer")
                        .WithMany("Reviews")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ecommerce.SharedViewModel.Models.Product", "Product")
                        .WithMany("Reviews")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Variant", b =>
                {
                    b.HasOne("Ecommerce.SharedViewModel.Models.Product", "Product")
                        .WithMany("Variants")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.VariantCart", b =>
                {
                    b.HasOne("Ecommerce.SharedViewModel.Models.Cart", "Cart")
                        .WithMany("VariantCarts")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ecommerce.SharedViewModel.Models.Variant", "Variant")
                        .WithMany("VariantCarts")
                        .HasForeignKey("VariantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Variant");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.VariantCategory", b =>
                {
                    b.HasOne("Ecommerce.SharedViewModel.Models.Category", "Category")
                        .WithMany("VariantCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ecommerce.SharedViewModel.Models.Variant", "Variant")
                        .WithMany("VariantCategories")
                        .HasForeignKey("VariantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Variant");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.VariantOrder", b =>
                {
                    b.HasOne("Ecommerce.SharedViewModel.Models.Order", "Order")
                        .WithMany("VariantOrders")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ecommerce.SharedViewModel.Models.Variant", "Variant")
                        .WithMany("VariantOrders")
                        .HasForeignKey("VariantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Variant");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Cart", b =>
                {
                    b.Navigation("VariantCarts");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Category", b =>
                {
                    b.Navigation("VariantCategories");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Classification", b =>
                {
                    b.Navigation("ProductClassifications");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Customer", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("Reviews");

                    b.Navigation("cart")
                        .IsRequired();
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Order", b =>
                {
                    b.Navigation("VariantOrders");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.ParentCategory", b =>
                {
                    b.Navigation("Categories");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Product", b =>
                {
                    b.Navigation("ProductClassifications");

                    b.Navigation("Reviews");

                    b.Navigation("Variants");
                });

            modelBuilder.Entity("Ecommerce.SharedViewModel.Models.Variant", b =>
                {
                    b.Navigation("VariantCarts");

                    b.Navigation("VariantCategories");

                    b.Navigation("VariantOrders");
                });
#pragma warning restore 612, 618
        }
    }
}
