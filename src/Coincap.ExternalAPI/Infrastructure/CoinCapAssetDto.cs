using System.Text.Json.Serialization;

namespace Coincap.ExternalAPI.Infrastructure;

// Respuesta JSON de CoinCap GET /assets
// Todos los valores numéricos son string en el api
public class CoinCapAssetsResponse
{
    [JsonPropertyName("data")]
    public List<CoinCapAssetDto> Data { get; set; } = [];

    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
}

public class CoinCapAssetDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("rank")]
    public string Rank { get; set; } = string.Empty;

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("priceUsd")]
    public string? PriceUsd { get; set; }

    [JsonPropertyName("marketCapUsd")]
    public string? MarketCapUsd { get; set; }

    [JsonPropertyName("volumeUsd24Hr")]
    public string? VolumeUsd24Hr { get; set; }

    [JsonPropertyName("changePercent24Hr")]
    public string? ChangePercent24Hr { get; set; }
}
