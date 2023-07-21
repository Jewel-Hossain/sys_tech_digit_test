//In the name of Allah



namespace SysTech.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }//constructor

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<InventoryProductMap> InventoryProductMaps => Set<InventoryProductMap>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new CustomerConfiguration().Configure(modelBuilder.Entity<Customer>());
        new ProductConfiguration().Configure(modelBuilder.Entity<Product>());
    }//func

}//class