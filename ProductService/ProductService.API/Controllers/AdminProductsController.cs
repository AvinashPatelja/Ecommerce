using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.DTOs;
using ProductService.Application.Services;

[ApiController]
[Route("admin/products")]
public class AdminProductController : ControllerBase
{
    private readonly IProductService _productService;

    public AdminProductController(IProductService productService)
    {
        _productService = productService;
    }

    // 1. Create product
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        await _productService.CreateAsync(dto);
        return Ok();
    }

    // 2. Update Name, Description, Price
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDetails(Guid id, UpdateProductDetailsDto dto)
    {
        await _productService.UpdateDetailsAsync(id, dto);
        return Ok();
    }

    // 3. Update IsActive (soft delete / enable)
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateProductStatusDto dto)
    {
        await _productService.UpdateStatusAsync(id, dto.IsActive);
        return Ok();
    }

    // 4. Admin product list
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _productService.GetAllAsync());
    }
}
