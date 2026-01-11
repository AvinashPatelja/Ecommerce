using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTOs;
using OrderService.Application.Services;

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

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
    {
        var userId = Guid.Parse(User.FindFirst("sub")!.Value);

        var orderId = await _orderService.CreateOrderAsync(userId, request);
        return Ok(new { OrderId = orderId });
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
