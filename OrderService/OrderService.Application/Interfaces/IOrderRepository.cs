using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces;

public interface IOrderRepository
{
    Task AddOrderAsync(Order order);
    Task<Order?> GetOrderByIdAsync(Guid orderId);
    Task<List<Order>> GetOrdersByUserIdAsync(Guid userId);
    Task SaveChangesAsync();
}
