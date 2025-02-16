using cakeDelivery.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cakeDelivery.DataAccess;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.CategoryId);

        builder.Property(c => c.CategoryName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.CategoryImageUrl)
            .HasMaxLength(255);
    }
}