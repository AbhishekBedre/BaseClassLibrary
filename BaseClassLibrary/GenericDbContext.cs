using Microsoft.EntityFrameworkCore;

namespace BaseClassLibrary
{
    public class GenericDbContext<TContext> : DbContext where TContext : DbContext
    {        
        public GenericDbContext(DbContextOptions<TContext> options)
            : base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {         
            base.OnModelCreating(modelBuilder);
        }
    }
}