namespace InventoryService.Application.DTOs;

public class UpdateInventoryDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
