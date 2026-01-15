using ProductService.Application.DTOs;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(CreateProductDto dto)
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

            await _repository.AddAsync(product);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return (await _repository.GetAllAsync()).ToList();
        }
        public async Task UpdateDetailsAsync(Guid id, UpdateProductDetailsDto dto)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) throw new Exception("Product not found");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;

            await _repository.UpdateAsync(product);
        }

        public async Task UpdateStatusAsync(Guid id, bool isActive)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) throw new Exception("Product not found");

            product.IsActive = isActive;

            await _repository.UpdateAsync(product);
        }

    }
}
