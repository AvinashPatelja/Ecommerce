using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Application.Services
{
    public interface IInventoryService
    {
        Task<int> GetStockAsync(Guid productId);
        // Admin
        Task UpdateStockByAdminAsync(Guid productId, int quantity);
        // Orders
        Task ReduceStockByOrderAsync(Guid productId, int orderedQty);
    }
}
