namespace OrderService.Application.Interfaces;

public interface IInventoryClient
{
    Task<bool> UpdateInventoryAsync(Guid productId, int quantity);
}
