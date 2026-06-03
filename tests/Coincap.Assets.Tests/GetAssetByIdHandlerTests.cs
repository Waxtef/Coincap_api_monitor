using Coincap.Assets.Application.Queries.GetAssetById;
using Coincap.Assets.Domain;
using Moq;

namespace Coincap.Assets.Tests;

public class GetAssetByIdHandlerTests
{
    // Simula la base de datos sin tocar la real
    private readonly Mock<IAssetRepository> _repositoryMock = new();
    private readonly GetAssetByIdHandler _handler;

    public GetAssetByIdHandlerTests()
    {
        _handler = new GetAssetByIdHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenAssetExists_ReturnsSuccess()
    {
        // Retornamos bitcoin como activo existente
        var asset = new Asset("bitcoin", "BTC", "Bitcoin", 1, 68000m, 1_300_000_000m, 44_000_000m, -2.5m);
        _repositoryMock
            .Setup(r => r.GetByIdAsync("bitcoin", default))
            .ReturnsAsync(asset);

        var result = await _handler.Handle(new GetAssetByIdQuery("bitcoin"), default);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("bitcoin", result.Value.Id);
        Assert.Equal("BTC", result.Value.Symbol);
        Assert.Equal(68000m, result.Value.PriceUsd);
    }

    [Fact]
    public async Task Handle_WhenAssetDoesNotExist_ReturnsFailure()
    {
        // Repositorio retorna null — activo no encontrado
        _repositoryMock
            .Setup(r => r.GetByIdAsync("nonexistent", default))
            .ReturnsAsync((Asset?)null);

        var result = await _handler.Handle(new GetAssetByIdQuery("nonexistent"), default);

        // Debe retornar Failure para que el controller retorne 404
        Assert.True(result.IsFailure);
        Assert.Contains("nonexistent", result.Error);
    }

    [Fact]
    public async Task Handle_WhenAssetExists_MapsAllFieldsCorrectly()
    {
        // Verifica que todos los campos se mapean correctamente de entidad a DTO
        var asset = new Asset("bitcoin", "BTC", "Bitcoin", 1, 68000m, 1_300_000_000m, 44_000_000m, -2.5m);
        _repositoryMock
            .Setup(r => r.GetByIdAsync("bitcoin", default))
            .ReturnsAsync(asset);

        var result = await _handler.Handle(new GetAssetByIdQuery("bitcoin"), default);

        Assert.True(result.IsSuccess);
        Assert.Equal("bitcoin", result.Value!.Id);
        Assert.Equal("BTC", result.Value.Symbol);
        Assert.Equal("Bitcoin", result.Value.Name);
        Assert.Equal(1, result.Value.Rank);
        Assert.Equal(68000m, result.Value.PriceUsd);
    }
}
