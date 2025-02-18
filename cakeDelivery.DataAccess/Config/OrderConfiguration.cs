using cakeDelivery.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cakeDelivery.DataAccess;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.OrderId);

        builder.Property(o => o.TotalAmount)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(o => o.OrderDate)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(o => o.PaymentStatus)
            .HasMaxLength(20)
            .HasConversion<string>();

        builder.Property(o => o.DeliveryStatus)
            .HasMaxLength(20)
            .HasConversion<string>();

        builder.HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
