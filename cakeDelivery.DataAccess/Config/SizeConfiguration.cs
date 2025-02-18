using cakeDelivery.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cakeDelivery.DataAccess;

public class SizeConfiguration : IEntityTypeConfiguration<Size>
{
    public void Configure(EntityTypeBuilder<Size> builder)
    {

        builder.HasKey(s => s.SizeId);

        builder.Property(s => s.SizeName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(s => s.PriceMultiplier)
            .HasColumnType("decimal(8,2)")
            .IsRequired();
    }
}
