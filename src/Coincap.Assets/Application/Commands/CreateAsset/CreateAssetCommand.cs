using Coincap.Share;
using MediatR;

namespace Coincap.Assets.Application.Commands.CreateAsset;

// Command que representa la intención de insertar un activo nuevo en la base de datos
public record CreateAssetCommand(
    string Id,
    string Symbol,
    string Name,
    int Rank,
    decimal PriceUsd,
    decimal MarketCapUsd,
    decimal VolumeUsd24Hr,
    decimal ChangePercent24Hr
) : IRequest<Result>;
