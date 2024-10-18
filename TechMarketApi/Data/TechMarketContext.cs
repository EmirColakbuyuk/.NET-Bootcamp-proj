using Microsoft.EntityFrameworkCore;
using TechMarketApi.Models;

namespace TechMarketApi.Data
{
    public class TechMarketContext : DbContext
    {
        public TechMarketContext(DbContextOptions<TechMarketContext> options) : base(options) { }

        public DbSet<Computer> Computers { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Smartwatch> Smartwatches { get; set; }
    }
}
