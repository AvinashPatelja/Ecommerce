using System.Net.Http.Json;
using OrderService.Application.Interfaces;

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
}
