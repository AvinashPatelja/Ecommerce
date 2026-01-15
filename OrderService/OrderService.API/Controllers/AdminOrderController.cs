using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTOs;
using OrderService.Application.Services;

namespace OrderService.API.Controllers
{
    [ApiController]
    [Route("api/admin/orders")]
    public class AdminOrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public AdminOrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // 1️⃣ List all orders
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        // 2️⃣ Get order details
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            return Ok(order);
        }

        // 3️⃣ Update order status
        [HttpPatch("{orderId}/status")]
        public async Task<IActionResult> UpdateStatus(
            Guid orderId,
            UpdateOrderStatusDto dto)
        {
            await _orderService.UpdateOrderStatusAsync(orderId, dto.Status);
            return Ok();
        }
    }

}
