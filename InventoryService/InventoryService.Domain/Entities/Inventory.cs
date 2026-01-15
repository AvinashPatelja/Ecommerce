namespace InventoryService.Domain.Entities;

public class Inventory
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int AvailableQuantity { get; set; }
    public DateTime LastUpdatedOn { get; set; }
    public string AuditDescription { get; set; } = default!;
}
