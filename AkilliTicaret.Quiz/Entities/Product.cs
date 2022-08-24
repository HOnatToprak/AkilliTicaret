namespace AkilliTicaret.Entity;

public class Product
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = default!;

    public List<OrderProduct> OrderProducts { get; set; } = default!;
}
