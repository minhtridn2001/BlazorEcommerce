using System.Net.Http.Json;
using BlazorEcommerce.Shared.DTO;

namespace BlazorEcommerce.Shared.ClientServices;

public class ProductService
{
    private readonly HttpClient _http;

    public ProductService(HttpClient http)
    {
        _http = http;
    }

    public async Task<ProductDTO?> GetProductByIdAsync(string id)
    {
        try
        {
            return await _http.GetFromJsonAsync<ProductDTO>($"api/products/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting product: {ex}");
            return null;
        }
    }
}