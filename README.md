# BaseClassLibrary
Provides all basic EF Core functionality so that user can pass only Entity Model and they can get CRUD operations along with Executing SP, Paganing etc

#Sample
1. Base model for all your models
public class BaseModel
{
    public DateTime? CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid ModifiedBy { get; set; }
}

2. Sample model inn your application
public class CompanyMaster : BasseModel
{
    public Int64 Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? GSTNo { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid CreatedUserId { get; set; }
}

3. Create your AppDbContext like below
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

4. Register the service like this.

...
builder.Services.AddBaseLibraryServices();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(@"Connection string"));
...
