using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Persistence.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly InventoryDbContext _context;

    public InventoryRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Inventory?> GetByProductIdAsync(Guid productId)
    {
        return await _context.Inventories
            .FirstOrDefaultAsync(i => i.ProductId == productId);
    }

    public async Task AddAsync(Inventory inventory)
    {
        _context.Inventories.Add(inventory);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Inventory inventory)
    {
        _context.Inventories.Update(inventory);
        await _context.SaveChangesAsync();
    }
}
