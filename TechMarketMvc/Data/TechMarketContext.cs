using Microsoft.EntityFrameworkCore;
using TechMarketMvc.Models;

namespace TechMarketMvc.Data
{
    public class TechMarketContext : DbContext
    {
        public TechMarketContext(DbContextOptions<TechMarketContext> options)
            : base(options)
        {
        }

        public DbSet<Computer> Computers { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Smartwatch> Smartwatches { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed an admin user (Note: Passwords should be hashed in production)
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Username = "admin",
                Password = "admin123",  // Store a hashed password in a real-world application
                Role = "Admin"
            });
        }
    }
}
