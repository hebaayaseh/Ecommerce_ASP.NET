using Ecommerce_ASP.NET.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ecommerce_ASP.NET.Data
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.role)
                .HasConversion<string>();
            modelBuilder.Entity<Orders>()
                .Property(u => u.status)
                .HasConversion<string>();
        }
        
        public DbSet<Models.Products> Products { get; set; }
        public DbSet<Models.CartItems> CartItems { get; set; }
        public DbSet<Models.Orders> Orders { get; set; }
        public DbSet<Models.OrderItems> OrderItems { get; set; }
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Categories> Categories { get; set; }
    }
}
