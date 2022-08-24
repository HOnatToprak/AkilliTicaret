using Microsoft.EntityFrameworkCore;

namespace AkilliTicaret.Quiz;

public class AkilliTicaretDbContext : DbContext
{
    public AkilliTicaretDbContext(DbContextOptions<AkilliTicaretDbContext> options)
      : base(options)
    {
    }

    public DbSet<Entity.Order> Orders { get; set; } = default!;
    public DbSet<Entity.Product> Products { get; set; } = default!;
    public DbSet<Entity.OrderProduct> OrderProducts { get; set; } = default!;
    public DbSet<Entity.Category> Categories { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
          .ApplyConfigurationsFromAssembly(typeof(AkilliTicaretDbContext).Assembly);
    }
}
