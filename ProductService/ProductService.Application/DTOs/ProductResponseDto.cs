namespace ProductService.Application.DTOs;

public class ProductResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal? Price { get; set; }   // nullable on purpose
    public string? ImageUrl { get; set; }
    public bool IsPriceVisible { get; set; }
}
