using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;
using System.Net.Http.Json;

public class ProductClient : IProductClient
{
    private readonly HttpClient _httpClient;

    public ProductClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Dictionary<Guid, string>> GetProductNamesAsync(IEnumerable<Guid> productIds)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "/products/names",
            productIds);

        return await response.Content.ReadFromJsonAsync<Dictionary<Guid, string>>();
    }
    public async Task<List<ProductDto>> GetProductsAsync(IEnumerable<Guid> productIds)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "/products/products",
            productIds);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<ProductDto>>();
    }
}
