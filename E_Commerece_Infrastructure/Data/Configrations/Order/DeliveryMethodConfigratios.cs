using E_Commerece.Domain.Entites.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Infrastructure.Data.Configrations.Order
{
    internal class DeliveryMethodConfigratios : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(p => p.DeliveryTime)
               .HasMaxLength(250);
            builder.Property(p => p.ShortName)
               .HasMaxLength(250);
            builder.Property(p => p.Description)
               .HasMaxLength(250);

            builder.Property(p => p.Price)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

        }
    }
}
