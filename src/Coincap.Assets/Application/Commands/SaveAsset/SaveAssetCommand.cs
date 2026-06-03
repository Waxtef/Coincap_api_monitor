using Coincap.Share;
using MediatR;

namespace Coincap.Assets.Application.Commands.SaveAsset;

// Punto de entrada para guardar un activo — decide internamente si crear o actualizar
// Es el único Command que conoce el módulo ExternalAPI para persistir activos
public record SaveAssetCommand(
    string Id,
    string Symbol,
    string Name,
    int Rank,
    decimal PriceUsd,
    decimal MarketCapUsd,
    decimal VolumeUsd24Hr,
    decimal ChangePercent24Hr
) : IRequest<Result>;
