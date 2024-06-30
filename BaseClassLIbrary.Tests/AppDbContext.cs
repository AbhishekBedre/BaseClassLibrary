using BaseClassLibrary;
using BaseClassLIbrary.Tests.Entity;
using Microsoft.EntityFrameworkCore;

namespace BaseClassLIbrary.Tests
{
    public class AppDbContext : GenericDbContext<AppDbContext>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Add DbSet properties for your entities
        public DbSet<CompanyMaster> CompanyMaster { get; set; }

        // You can override OnModelCreating if you need additional configurations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Additional configurations for your entities
            modelBuilder.Entity<CompanyMaster>().HasKey(c => c.Id);
        }
    }
}
