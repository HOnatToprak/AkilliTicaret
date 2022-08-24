using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AkilliTicaret.Entity;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public decimal Price { get; set; }

    public List<OrderProduct> OrderProducts { get; set; } = default!;
}

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder
            .Property(b => b.Price)
            .HasPrecision(19, 4);
    }
}
