using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;

namespace OrderService.Application.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IInventoryClient _inventoryClient;

    public OrderService(
        IOrderRepository orderRepository,
        IInventoryClient inventoryClient)
    {
        _orderRepository = orderRepository;
        _inventoryClient = inventoryClient;
    }

    public async Task<Guid> CreateOrderAsync(Guid userId, CreateOrderRequest request)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Status = OrderStatus.Created.ToString(),
            CreatedOn = DateTime.UtcNow,
            Items = new List<OrderItem>()
        };

        decimal totalAmount = 0;

        foreach (var item in request.Items)
        {
            // Price will later come from Product Service
            decimal price = 100; // temporary placeholder

            order.Items.Add(new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = price
            });

            totalAmount += price * item.Quantity;
        }

        order.TotalAmount = totalAmount;

        await _orderRepository.AddOrderAsync(order);
        await _orderRepository.SaveChangesAsync();

        // Reduce inventory
        foreach (var item in request.Items)
        {
            var success = await _inventoryClient.UpdateInventoryAsync(
                item.ProductId,
                -item.Quantity);

            if (!success)
            {
                order.Status = OrderStatus.Cancelled.ToString();
                await _orderRepository.SaveChangesAsync();
                throw new Exception("Inventory update failed");
            }
        }

        order.Status = OrderStatus.Confirmed.ToString();
        await _orderRepository.SaveChangesAsync();

        return order.Id;
    }
}
