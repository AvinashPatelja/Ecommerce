using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Application.Services
{
    public class InventoryServices : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        public InventoryServices(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }
        public async Task<int> GetStockAsync(Guid productId)
        {
            return await _inventoryRepository.GetStockAsync(productId);
        }

        public async Task ReduceStockByOrderAsync(Guid productId, int orderedQty)
        {
            var inventory = await _inventoryRepository.GetByProductIdAsync(productId);

            if (inventory == null)
                throw new Exception("Inventory not found");

            if (inventory.AvailableQuantity < orderedQty)
                throw new Exception("Insufficient stock");

            inventory.AvailableQuantity -= orderedQty;

            await _inventoryRepository.UpdateAsync(inventory);
        }

        public async Task UpdateStockByAdminAsync(Guid productId, int quantity)
        {
            var inventory = await _inventoryRepository.GetByProductIdAsync(productId);

            if (inventory == null)
            {
                inventory = new Inventory
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    AvailableQuantity = quantity,
                    AuditDescription = "Stock added by admin",
                    LastUpdatedOn = DateTime.UtcNow
                };
                await _inventoryRepository.AddAsync(inventory);
            }
            else
            {
                inventory.AvailableQuantity += quantity;
                inventory.LastUpdatedOn = DateTime.UtcNow;
                inventory.AuditDescription = "Stock added by admin";
                await _inventoryRepository.UpdateAsync(inventory);
            }
        }
    }
}
