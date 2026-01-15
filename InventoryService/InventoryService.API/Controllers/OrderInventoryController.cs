using InventoryService.Application.DTOs;
using InventoryService.Application.Interfaces;
using InventoryService.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.API.Controllers
{
    [Route("api/Order/Inventory")]
    [ApiController]
    public class OrderInventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        public OrderInventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpPost("update")]
        public async Task<IActionResult> ReduceStockByOrderAsync(UpdateInventoryDto dto)
        {
            await _inventoryService.ReduceStockByOrderAsync(dto.ProductId, dto.Quantity);
            return Ok();
        }
    }
}
