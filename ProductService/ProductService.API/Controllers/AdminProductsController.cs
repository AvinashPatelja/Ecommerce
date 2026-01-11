using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.DTOs;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.API.Controllers
{
    [ApiController]
    [Route("api/admin/products")]
    //[Authorize(Roles = "Admin")]
    public class AdminProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;

        public AdminProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                IsActive = true,
                CreatedOn = DateTime.UtcNow
            };

            await _repo.AddAsync(product);
            return Ok(product);
        }
    }

}
