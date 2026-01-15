using OrderService.Domain.Entities;
using OrderService.Domain.Enums;

namespace OrderService.Application.Interfaces;

public interface IOrderRepository
{
    Task AddOrderAsync(Order order);
    Task<Order?> GetOrderByIdAsync(Guid orderId);
    Task<List<Order>> GetOrdersByUserIdAsync(Guid userId);
    Task SaveChangesAsync();
    Task<List<Order>> GetAllOrdersAsync();
    Task UpdateOrderStatusAsync(Order order);
}
