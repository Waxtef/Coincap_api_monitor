using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Coincap.ExternalAPI.Infrastructure;

// Cliente HTTP que se conecta a la API de CoinCap
// Polly (retry + circuit breaker) se configura en Program.cs al registrar el HttpClient
public class CoinCapClient
{
    private readonly HttpClient _httpClient;
    private readonly CoinCapOptions _options;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public CoinCapClient(HttpClient httpClient, IOptions<CoinCapOptions> options)
    {
        _options = options.Value;
        _httpClient = httpClient;

        // Solo configura el header de autenticación — la URL completa se construye en cada método
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_options.ApiKey}");
    }

    // Trae los primeros 100 activos ordenados por market cap desde CoinCap
    // Se usa la URL completa para evitar el comportamiento de BaseAddress que elimina el segmento /v3
    public async Task<List<CoinCapAssetDto>> GetAssetsAsync(CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync($"{_options.BaseUrl}/assets?limit={_options.AssetLimit}", ct);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(ct);
        var result = JsonSerializer.Deserialize<CoinCapAssetsResponse>(content, JsonOptions);

        return result?.Data ?? [];
    }
}
