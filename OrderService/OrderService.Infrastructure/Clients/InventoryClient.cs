using Microsoft.AspNetCore.Http;
using OrderService.Application.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace OrderService.Persistence.Clients;

public class InventoryClient : IInventoryClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public InventoryClient(
        HttpClient httpClient,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> UpdateInventoryAsync(Guid productId, int quantity)
    {
        // 🔑 Forward Authorization header
        var authHeader = _httpContextAccessor.HttpContext?
            .Request.Headers["Authorization"]
            .ToString();

        if (!string.IsNullOrEmpty(authHeader))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse(authHeader);
        }

        var response = await _httpClient.PostAsJsonAsync(
            "/order/inventory/update",
            new
            {
                ProductId = productId,
                Quantity = quantity
            });

        return response.IsSuccessStatusCode;
    }
}
