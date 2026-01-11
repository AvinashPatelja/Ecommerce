using InventoryService.Domain.Entities;

namespace InventoryService.Application.Interfaces;

public interface IInventoryRepository
{
    Task<Inventory?> GetByProductIdAsync(Guid productId);
    Task AddAsync(Inventory inventory);
    Task UpdateAsync(Inventory inventory);
}
