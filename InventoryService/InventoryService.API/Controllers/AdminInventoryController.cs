using InventoryService.Application.DTOs;
using InventoryService.Application.Interfaces;
using InventoryService.Application.Services;
using InventoryService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/admin/inventory")]
//[Authorize(Roles = "Admin")]
public class AdminInventoryController : ControllerBase
{
    private readonly IInventoryRepository _repo;
    public AdminInventoryController(IInventoryRepository repo)
    {
        _repo = repo;
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateInventory(UpdateInventoryDto dto)
    {
        var inventory = await _repo.GetByProductIdAsync(dto.ProductId);

        if (inventory == null)
        {
            inventory = new Inventory
            {
                Id = Guid.NewGuid(),
                ProductId = dto.ProductId,
                AvailableQuantity = dto.Quantity,
                AuditDescription = "Stock added by admin",
                LastUpdatedOn = DateTime.UtcNow
            };
            await _repo.AddAsync(inventory);
        }
        else
        {
            inventory.AvailableQuantity += dto.Quantity;
            inventory.LastUpdatedOn = DateTime.UtcNow;
            inventory.AuditDescription = "Stock added by admin";
            await _repo.UpdateAsync(inventory);
        }

        return Ok(inventory);
    }    

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetStock(Guid productId)
    {
        var inventory = await _repo.GetByProductIdAsync(productId);
        return Ok(inventory?.AvailableQuantity ?? 0);
    }
}
