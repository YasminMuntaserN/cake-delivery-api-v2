using cakeDelivery.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cakeDelivery.DataAccess;

    public class CakeConfiguration : IEntityTypeConfiguration<Cake>
    {
        public void Configure(EntityTypeBuilder<Cake> builder)
        {
            builder.ToTable("Cakes");
            
            builder.HasKey(c => c.CakeId);

            builder.Property(c => c.CakeName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Description)
                .HasMaxLength(500);

            builder.Property(c => c.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(c => c.StockQuantity)
                .IsRequired();

            builder.Property(c => c.ImageUrl)
                .HasMaxLength(500);

            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(c => c.Category)
                .WithMany(c => c.Cakes)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
