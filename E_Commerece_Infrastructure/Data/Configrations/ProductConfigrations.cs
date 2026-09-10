using E_Commerece.Domain.Entites.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Infrastructure.Data.Configrations
{
    public class ProductConfigrations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            
            builder.HasOne(X => X.ProductBrand).WithMany().HasForeignKey(X => X.BrandId);
            builder.HasOne(X => X.ProductType).WithMany().HasForeignKey(X => X.TypeId);

            builder.Property(X=>X.Price).HasColumnType("decimal(18,2)");
            builder.Property(X => X.Name).HasMaxLength(100);
            builder.Property(X => X.Description).HasMaxLength(500);
            builder.Property(X => X.PictureUrl).HasMaxLength(200);



        }
    }
}
