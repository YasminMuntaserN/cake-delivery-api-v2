using cakeDelivery.Entities;
using Microsoft.EntityFrameworkCore;

namespace cakeDelivery.DataAccess.Data;

public class AppDbContext : DbContext
{ 
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


    public DbSet<Cake> Cakes { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerFeedback> CustomerFeedbacks { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

}