using ProductService.Domain.Entities;

namespace ProductService.Application.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<Product>> GetActiveProductsAsync();
}
