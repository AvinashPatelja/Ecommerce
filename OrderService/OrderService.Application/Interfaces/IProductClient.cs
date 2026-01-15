using OrderService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Interfaces
{
    public interface IProductClient
    {
        Task<Dictionary<Guid, string>> GetProductNamesAsync(IEnumerable<Guid> productIds);
        Task<List<ProductDto>> GetProductsAsync(IEnumerable<Guid> productIds);
    }
}
