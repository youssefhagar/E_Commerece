using E_Commerece.Domain.Entites.Orders;
using E_Commerece.Domain.Entites.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Infrastructure.Data
{
    public class StoreDbContext(DbContextOptions<StoreDbContext> options) : DbContext(options)
    {



        public DbSet<Product> Products { get; set; } 
        public DbSet<ProductType> ProductBrands { get; set; }
        public DbSet<ProductBrand> ProductTypes { get; set; } 
        public DbSet<Order> Orders { get; set; } 
        public DbSet<OrderItem> OrderItems { get; set; } 
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; } 



        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreDbContext).Assembly);
        }
    }
}
