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
            var authHeader = Request.Headers["Authorization"].ToString();
            var test = User.Identity?.IsAuthenticated;
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
        [HttpPost("debug")]
        public IActionResult Debug()
        {
            return Ok(new
            {
                IsAuthenticated = User.Identity?.IsAuthenticated,
                Claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }
    }
}
