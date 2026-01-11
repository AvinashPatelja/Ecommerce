using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.DTOs;
using ProductService.Application.Interfaces;

namespace ProductService.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;

        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _repo.GetActiveProductsAsync();

            var isApprovedClaim = User.Claims
                .FirstOrDefault(c => c.Type == "isApproved")?.Value;

            bool isApprovedUser = isApprovedClaim == "True";

            var response = products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = isApprovedUser ? p.Price : null,
                IsPriceVisible = isApprovedUser
            });

            return Ok(response);
        }
    }
}
