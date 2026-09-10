
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace E_Commerece.Infrastructure.Data.Configrations.Order
{
    public class OrderConfigratios : IEntityTypeConfiguration<E_Commerece.Domain.Entites.Orders.Order>
    {

        public void Configure(EntityTypeBuilder<Domain.Entites.Orders.Order> builder)
        {
            builder.HasMany(p => p.Items)
                .WithOne()
                .HasForeignKey(X => X.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.DeliveryMethod)
                .WithMany()
                .HasForeignKey(X => X.DeliveryMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsOne(O => O.Address);

            builder.Property(p => p.PaymentStatu)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(p => p.Email)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.SubTotal)
                .HasColumnType("decimal(10,2)")
                .IsRequired();



        }
    }
}
