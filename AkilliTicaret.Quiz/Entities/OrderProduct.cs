using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AkilliTicaret.Entity;

// Order *--- Many To Many ---* Product
public class OrderProduct
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = default!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = default!;

    public decimal Price { get; set; }
}

public class OrderProductEntityTypeConfiguration : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder
            .Property(b => b.Price)
            .HasPrecision(19, 4);

        builder.HasOne(op => op.Product)
          .WithMany(b => b.OrderProducts)
          .HasForeignKey(op => op.ProductId);

        builder.HasOne(op => op.Order)
          .WithMany(b => b.OrderProducts)
          .HasForeignKey(op => op.OrderId);

    }
}
