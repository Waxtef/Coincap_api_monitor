using Coincap.Assets.Application.Queries.GetAssets;
using Coincap.Assets.Domain;
using Moq;

namespace Coincap.Assets.Tests;

public class GetAssetsHandlerTests
{
    private readonly Mock<IAssetRepository> _repositoryMock = new();
    private readonly GetAssetsHandler _handler;

    public GetAssetsHandlerTests()
    {
        _handler = new GetAssetsHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenAssetsExist_ReturnsMappedList()
    {
        // Retorna 2 activos — bitcoin y ethereum
        var assets = new List<Asset>
        {
            new Asset("bitcoin", "BTC", "Bitcoin", 1, 68000m, 1_300_000_000m, 44_000_000m, -2.5m),
            new Asset("ethereum", "ETH", "Ethereum", 2, 1973m, 238_000_000m, 12_000_000m, -0.11m)
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(1, 20, null, default))
            .ReturnsAsync((assets, 2));

        var result = await _handler.Handle(new GetAssetsQuery(1, 20, null), default);

        Assert.Equal(2, result.Total);
        Assert.Equal(2, result.Data.Count());
    }

    [Fact]
    public async Task Handle_WhenNoAssetsExist_ReturnsEmptyList()
    {
        // Lista vacía — no debe fallar
        _repositoryMock
            .Setup(r => r.GetAllAsync(1, 20, null, default))
            .ReturnsAsync((new List<Asset>(), 0));

        var result = await _handler.Handle(new GetAssetsQuery(1, 20, null), default);

        Assert.Equal(0, result.Total);
        Assert.Empty(result.Data);
    }

    [Fact]
    public async Task Handle_WhenSearchProvided_PassesSearchToRepository()
    {
        // Verifica que el parámetro search llega correctamente al repositorio
        _repositoryMock
            .Setup(r => r.GetAllAsync(1, 20, "bitcoin", default))
            .ReturnsAsync((new List<Asset>(), 0));

        await _handler.Handle(new GetAssetsQuery(1, 20, "bitcoin"), default);

        _repositoryMock.Verify(r => r.GetAllAsync(1, 20, "bitcoin", default), Times.Once);
    }
}
