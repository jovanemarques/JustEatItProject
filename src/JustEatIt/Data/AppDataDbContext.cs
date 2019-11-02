using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JustEatIt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace JustEatIt.Data
{
    public class AppDataDbContext : DbContext
    {
        public AppDataDbContext(DbContextOptions<AppDataDbContext> options) : base(options)
        {

        }

        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<Dish> Dish { get; set; }
        public DbSet<Partner> Partner { get; set; }
    }
}
