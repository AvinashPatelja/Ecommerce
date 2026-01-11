namespace OrderService.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Status { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedOn { get; set; }

    public ICollection<OrderItem> Items { get; set; }
}
