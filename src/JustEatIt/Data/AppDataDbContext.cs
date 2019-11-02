using JustEatIt.Models;
using Microsoft.EntityFrameworkCore;

namespace JustEatIt.Data
{
    public class AppDataDbContext : DbContext
    {
        public AppDataDbContext(DbContextOptions<AppDataDbContext> options) : base(options)
        {

        }

        public DbSet<ContactUs> ContactUs { get; set; }
<<<<<<< HEAD

        public DbSet<Dish> Dish { get; set; }

        public DbSet<Partner> Partner { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dish>()
                .Ignore(b => b.File);
        }
=======
        public DbSet<Dish> Dish { get; set; }
        public DbSet<Partner> Partner { get; set; }
>>>>>>> jovane_r1_i2
    }
}