using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;

namespace OrderService.Application.Services;

public class OrderServices : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IInventoryClient _inventoryClient;
    private readonly IProductClient _productClient;

    public OrderServices(
        IOrderRepository orderRepository,
        IInventoryClient inventoryClient,
        IProductClient productClient)
    {
        _orderRepository = orderRepository;
        _inventoryClient = inventoryClient;
        _productClient = productClient;
    }

    public async Task<Guid> CreateOrderAsync(Guid userId, CreateOrderRequest request)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            OrderStatus = OrderStatus.Created.ToString(),
            CreatedOn = DateTime.UtcNow,
            Items = new List<OrderItem>()
        };

        decimal totalAmount = 0;

        // 1️⃣ Collect productIds ONCE
        var productIds = request.Items
            .Select(i => i.ProductId)
            .Distinct()
            .ToList();

        // 2️⃣ SINGLE call to Product Service
        var products = await _productClient.GetProductsAsync(productIds);

        var productMap = products.ToDictionary(p => p.Id);

        // 3️⃣ Build order items using REAL price
        foreach (var item in request.Items)
        {
            if (!productMap.TryGetValue(item.ProductId, out var product))
                throw new Exception("Product not found");

            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = product.Price // ✅ FIX: real price from Product Service
            };

            order.Items.Add(orderItem);
            totalAmount += product.Price * item.Quantity;
        }

        order.TotalAmount = totalAmount;

        // 4️⃣ Save order first (same as your current flow)
        await _orderRepository.AddOrderAsync(order);
        await _orderRepository.SaveChangesAsync();

        // 5️⃣ Reduce inventory (UNCHANGED LOGIC)
        foreach (var item in request.Items)
        {
            var success = await _inventoryClient.UpdateInventoryAsync(
                item.ProductId,
                item.Quantity);

            if (!success)
            {
                order.OrderStatus = OrderStatus.Cancelled.ToString();
                await _orderRepository.SaveChangesAsync();
                throw new Exception("Inventory update failed");
            }
        }

        // 6️⃣ Confirm order
        order.OrderStatus = OrderStatus.Confirmed.ToString();
        await _orderRepository.SaveChangesAsync();

        return order.Id;
    }
    public async Task<OrderDto?> GetOrderByIdAsync(Guid orderId, Guid userId)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);

        if (order == null || order.UserId != userId)
            return null;

        // 1️⃣ Collect productIds
        var productIds = order.Items
            .Select(i => i.ProductId)
            .Distinct()
            .ToList();

        // 2️⃣ Fetch ONLY product names
        var productNames = await _productClient.GetProductNamesAsync(productIds);

        // 3️⃣ Map DTO
        return MapToDto(order, productNames);
    }

    public async Task<List<OrderDto>> GetOrdersByUserAsync(Guid userId)
    {
        try
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            // 1️⃣ Collect all productIds from orders
            var productIds = orders
                .SelectMany(o => o.Items)
                .Select(i => i.ProductId)
                .Distinct()
                .ToList();

            // 2️⃣ Fetch product names from ProductService
            var productNames = await _productClient.GetProductNamesAsync(productIds);

            // 3️⃣ Map orders with product names
            return orders.Select(o => MapToDto(o, productNames)).ToList();
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    private static OrderDto MapToDto(
    Order order,
    Dictionary<Guid, string> productNames)
    {
        return new OrderDto
        {
            Id = order.Id,
            OrderStatus = order.OrderStatus,
            TotalAmount = order.TotalAmount,
            CreatedOn = order.CreatedOn,
            Items = order.Items.Select(i => new OrderItemDto
            {
                ProductId = i.ProductId,
                Name = productNames.ContainsKey(i.ProductId)
                    ? productNames[i.ProductId]
                    : "Unknown Product",
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllOrdersAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(Guid orderId)
    {
        return await _orderRepository.GetOrderByIdAsync(orderId);
    }

    public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);

        if (order == null)
            throw new Exception("Order not found");

        order.OrderStatus = status.ToString();

        await _orderRepository.UpdateOrderStatusAsync(order);
    }
}
