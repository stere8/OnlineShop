using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models;
using System;

namespace OnlineShop.Data
{
    public class OnlineShopDbContext : IdentityDbContext<ApplicationUser>
    {
        public OnlineShopDbContext(DbContextOptions<OnlineShopDbContext> options)
            : base(options)
        {
            Console.WriteLine("DbContext Created");
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Electronics" },
                new Category { CategoryId = 2, Name = "Clothing" },
                new Category { CategoryId = 3, Name = "Home Appliances" },
                new Category { CategoryId = 4, Name = "Books" },
                new Category { CategoryId = 5, Name = "Toys" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Name = "Laptop", Price = 1500.00M, Description = "High-performance laptop", ImageUrl = "laptop.jpg", StockQuantity = 10, CategoryId = 1 },
                new Product { ProductId = 2, Name = "Smartphone", Price = 800.00M, Description = "Latest smartphone with 5G", ImageUrl = "smartphone.jpg", StockQuantity = 15, CategoryId = 1 },
                new Product { ProductId = 3, Name = "Bluetooth Speaker", Price = 100.00M, Description = "Portable Bluetooth speaker with high-quality sound", ImageUrl = "speaker.jpg", StockQuantity = 20, CategoryId = 1 },
                new Product { ProductId = 4, Name = "Smartwatch", Price = 200.00M, Description = "Waterproof smartwatch with health tracking features", ImageUrl = "smartwatch.jpg", StockQuantity = 30, CategoryId = 1 },
                new Product { ProductId = 5, Name = "Headphones", Price = 75.00M, Description = "Noise-cancelling over-ear headphones", ImageUrl = "headphones.jpg", StockQuantity = 25, CategoryId = 1 },

                new Product { ProductId = 6, Name = "T-shirt", Price = 20.00M, Description = "100% cotton T-shirt", ImageUrl = "tshirt.jpg", StockQuantity = 50, CategoryId = 2 },
                new Product { ProductId = 7, Name = "Jeans", Price = 45.00M, Description = "Classic blue jeans", ImageUrl = "jeans.jpg", StockQuantity = 40, CategoryId = 2 },
                new Product { ProductId = 8, Name = "Jacket", Price = 100.00M, Description = "Warm winter jacket", ImageUrl = "jacket.jpg", StockQuantity = 15, CategoryId = 2 },
                new Product { ProductId = 9, Name = "Sneakers", Price = 60.00M, Description = "Comfortable running shoes", ImageUrl = "sneakers.jpg", StockQuantity = 35, CategoryId = 2 },
                new Product { ProductId = 10, Name = "Dress", Price = 80.00M, Description = "Elegant evening dress", ImageUrl = "dress.jpg", StockQuantity = 20, CategoryId = 2 },

                new Product { ProductId = 11, Name = "Microwave Oven", Price = 150.00M, Description = "Compact microwave oven", ImageUrl = "microwave.jpg", StockQuantity = 10, CategoryId = 3 },
                new Product { ProductId = 12, Name = "Refrigerator", Price = 1200.00M, Description = "Double-door refrigerator with smart features", ImageUrl = "refrigerator.jpg", StockQuantity = 5, CategoryId = 3 },
                new Product { ProductId = 13, Name = "Washing Machine", Price = 500.00M, Description = "Front-loading washing machine", ImageUrl = "washingmachine.jpg", StockQuantity = 8, CategoryId = 3 },
                new Product { ProductId = 14, Name = "Air Conditioner", Price = 400.00M, Description = "Energy-efficient air conditioner", ImageUrl = "airconditioner.jpg", StockQuantity = 7, CategoryId = 3 },
                new Product { ProductId = 15, Name = "Blender", Price = 30.00M, Description = "Multi-functional kitchen blender", ImageUrl = "blender.jpg", StockQuantity = 50, CategoryId = 3 },

                new Product { ProductId = 16, Name = "Novel - The Great Gatsby", Price = 10.00M, Description = "Classic novel by F. Scott Fitzgerald", ImageUrl = "greatgatsby.jpg", StockQuantity = 100, CategoryId = 4 },
                new Product { ProductId = 17, Name = "Science Textbook", Price = 50.00M, Description = "Comprehensive high school science textbook", ImageUrl = "sciencetextbook.jpg", StockQuantity = 30, CategoryId = 4 },
                new Product { ProductId = 18, Name = "Children's Story Book", Price = 15.00M, Description = "Illustrated story book for kids", ImageUrl = "storybook.jpg", StockQuantity = 70, CategoryId = 4 },
                new Product { ProductId = 19, Name = "Puzzle Game", Price = 25.00M, Description = "500-piece puzzle game", ImageUrl = "puzzle.jpg", StockQuantity = 60, CategoryId = 5 },
                new Product { ProductId = 20, Name = "Toy Car", Price = 10.00M, Description = "Remote-controlled toy car", ImageUrl = "toycar.jpg", StockQuantity = 45, CategoryId = 5 }
            );
        }

        public override void Dispose()
        {
            base.Dispose();
            Console.WriteLine("DbContext Disposed");
        }
    }
}
