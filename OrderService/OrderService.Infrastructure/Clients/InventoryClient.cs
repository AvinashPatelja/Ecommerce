using System.Net.Http.Json;
using OrderService.Application.Interfaces;

namespace OrderService.Persistence.Clients;

public class InventoryClient : IInventoryClient
{
    private readonly HttpClient _httpClient;

    public InventoryClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> UpdateInventoryAsync(Guid productId, int quantity)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "/admin/inventory/update",
            new
            {
                ProductId = productId,
                Quantity = quantity
            });

        return response.IsSuccessStatusCode;
    }
}
