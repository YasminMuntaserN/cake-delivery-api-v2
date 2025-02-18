using cakeDelivery.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cakeDelivery.DataAccess;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.OrderItemId);

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.Property(oi => oi.PricePerItem)
            .HasColumnType("decimal(8,2)")
            .IsRequired();

        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(oi => oi.Cake)
            .WithMany(c => c.OrderItems)
            .HasForeignKey(oi => oi.CakeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(oi => oi.Size)
            .WithMany(s => s.OrderItems)
            .HasForeignKey(oi => oi.SizeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}