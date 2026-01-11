using Microsoft.EntityFrameworkCore;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Persistence.Repositories;

public class ProductRepository
    : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(ProductDbContext context)
    : base(context) { }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        return await _context.Set<Product>().Where(p => p.IsActive).ToListAsync();
    }
}
