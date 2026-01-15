using ProductService.Application.DTOs;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Services
{
    public interface IProductService
    {
        Task CreateAsync(CreateProductDto dto);
        Task UpdateDetailsAsync(Guid id, UpdateProductDetailsDto dto);
        Task UpdateStatusAsync(Guid id, bool isActive);
        Task<List<Product>> GetAllAsync();
    }

}
