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

        //[HttpGet]
        //public async Task<IActionResult> GetProduct([FromBody] List<Guid> productId)
        //{
        //    var product = await _repo.GetByIdAsync(productId);
        //    if (product == null || !product.IsActive)
        //    {
        //        throw new Exception("Product not found");
        //    }
        //    var isApprovedClaim = User.Claims
        //        .FirstOrDefault(c => c.Type == "isApproved")?.Value;
        //    bool isApprovedUser = isApprovedClaim == "True";
        //    var response = new ProductResponseDto
        //    {
        //        Id = product.Id,
        //        Name = product.Name,
        //        Description = product.Description,
        //        Price = isApprovedUser ? product.Price : null,
        //        IsPriceVisible = isApprovedUser
        //    };
        //    return Ok(response);
        //}

        [HttpPost("names")]
        public async Task<IActionResult> GetProductNames([FromBody] List<Guid> productIds)
        {
            var products = await _repo.GetByIdsAsync(productIds);

            return Ok(products.ToDictionary(p => p.Id, p => p.Name));
        }

        [HttpPost("products")]
        public async Task<IActionResult> GetProducts([FromBody] List<Guid> productIds)
        {
            var products = await _repo.GetByIdsAsync(productIds);

            return Ok(products);
        }
    }
}
