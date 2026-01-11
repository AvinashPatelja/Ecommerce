namespace ProductService.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedOn { get; set; }
}
