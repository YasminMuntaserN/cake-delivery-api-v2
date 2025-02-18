using cakeDelivery.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cakeDelivery.DataAccess;

public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.HasKey(d => d.DeliveryId);

        builder.Property(d => d.DeliveryAddress)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(d => d.DeliveryCity)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(d => d.DeliveryPostalCode)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(d => d.DeliveryCountry)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(d => d.DeliveryStatus)
            .HasMaxLength(20)
            .HasConversion<string>();

        // Relationships
        builder.HasOne(d => d.Order)
            .WithMany(o => o.Deliveries)
            .HasForeignKey(d => d.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}