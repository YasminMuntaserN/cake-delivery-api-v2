using cakeDelivery.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cakeDelivery.DataAccess;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {

        builder.HasKey(p => p.PaymentId);

        builder.Property(p => p.PaymentMethod)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.PaymentDate)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(p => p.AmountPaid)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(p => p.PaymentStatus)
            .HasMaxLength(10)
            .HasConversion<string>();

        builder.HasOne(p => p.Order)
            .WithMany(o => o.Payments)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
