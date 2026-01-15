using OrderService.Application.DTOs;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Services
{
    public interface IOrderService
    {
        Task<Guid> CreateOrderAsync(Guid userId, CreateOrderRequest request);
        Task<OrderDto?> GetOrderByIdAsync(Guid orderId, Guid userId);
        Task<List<OrderDto>> GetOrdersByUserAsync(Guid userId);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(Guid orderId);
        Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
    }

}
