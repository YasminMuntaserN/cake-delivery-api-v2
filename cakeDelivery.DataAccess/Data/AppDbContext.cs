using cakeDelivery.Entities;
using Microsoft.EntityFrameworkCore;

namespace cakeDelivery.DataAccess.Data;

public class AppDbContext : DbContext
{ 
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


    public DbSet<Cake> Cakes { get; set; }
    public DbSet<Category> Categories { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

}