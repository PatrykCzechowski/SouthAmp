using Microsoft.EntityFrameworkCore;

namespace SouthAmp.Infrastructure.Data // poprawiona przestrzeń nazw
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Domyślny connection string do migracji lokalnych
                optionsBuilder.UseNpgsql("Host=localhost;Database=southampdb;Username=postgres;Password=yourpassword");
            }
        }
    }
}