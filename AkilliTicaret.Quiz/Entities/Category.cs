namespace AkilliTicaret.Entity;

public class Category
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public Category? Parent { get; set; } = default!;
}
