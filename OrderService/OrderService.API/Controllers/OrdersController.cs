using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTOs;
using OrderService.Application.Services;
using System.Security.Claims;

namespace OrderService.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly OrderService.Application.Services.OrderService _orderService;

    public OrdersController(OrderService.Application.Services.OrderService orderService)
    {
        _orderService = orderService;
    }
    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
    {
        var userId = Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value
        );
        var orderId = await _orderService.CreateOrderAsync(userId, request);
        return Ok(new { OrderId = orderId });
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrderById(Guid orderId)
    {
        var userId = Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value
        );
        var order = await _orderService.GetOrderByIdAsync(orderId, userId);

        if (order == null)
            return NotFound();

        return Ok(order);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value
        );
        var orders = await _orderService.GetOrdersByUserAsync(userId);

        return Ok(orders);
    }

    [HttpPost("debug")]
    public IActionResult Debug()
    {
        return Ok(new
        {
            IsAuthenticated = User.Identity?.IsAuthenticated,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }

}
