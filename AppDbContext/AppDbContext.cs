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
            modelBuilder.Entity<Payment>()
                .Property(u => u.Status)
                .HasConversion<string>();
            modelBuilder.Entity<Notification>()
                .Property(u => u.Status)
                .HasConversion<string>();
            modelBuilder.Entity<Discount>()
                .Property(u => u.Type)
                .HasConversion<string>();
        }
        public DbSet<Models.Address> addresses { get; set; }
        public DbSet<Models.Discount> discounts { get; set; }
        public DbSet<Models.Review> reviews { get; set; }
        public DbSet<Models.Payment> payments { get; set; }
        public DbSet<Models.Notification> notifications { get; set; }
        public DbSet<Models.Products> Products { get; set; }
        public DbSet<Models.CartItems> CartItems { get; set; }
        public DbSet<Models.Orders> Orders { get; set; }
        public DbSet<Models.OrderItems> OrderItems { get; set; }
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Categories> Categories { get; set; }
        public DbSet<Models.WishlistItems> wishlist { get; set; }
        public DbSet<Models.OrderTrackings> OrderTrackings { get; set; }
        public DbSet<Models.TrackingHistories> TrackingHistories { get; set; }

        }
}
