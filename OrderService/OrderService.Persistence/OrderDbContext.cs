using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using System.Collections.Generic;

namespace OrderService.Persistence.DbContexts;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options)
        : base(options) { }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}
