using JustEatIt.Models;
using Microsoft.EntityFrameworkCore;

namespace JustEatIt.Data
{
    public class AppDataDbContext : DbContext
    {
        public AppDataDbContext(DbContextOptions<AppDataDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Partner>().Property(p => p.Longitude).HasColumnType("decimal(9, 6)");
            modelBuilder.Entity<Partner>().Property(p => p.Latitude).HasColumnType("decimal(8, 6)");
        }

        public DbSet<ContactUs> ContactUs { get; set; }

        public DbSet<Dish> Dishes { get; set; }

        public DbSet<DishType> DishTypes { get; set; }

        public DbSet<DishAvailability> DishesAvail { get; set; }

        public DbSet<Partner> Partners { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
    }
}