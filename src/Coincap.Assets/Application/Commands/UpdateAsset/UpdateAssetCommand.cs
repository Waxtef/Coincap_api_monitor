using Coincap.Assets.Domain;
using Coincap.Share;
using MediatR;

namespace Coincap.Assets.Application.Commands.UpdateAsset;

// Command que representa la intención de actualizar un activo que ya existe en la base de datos
// Recibe la entidad existente para poder comparar el precio anterior con el nuevo
public record UpdateAssetCommand(
    Asset ExistingAsset,
    int Rank,
    decimal PriceUsd,
    decimal MarketCapUsd,
    decimal VolumeUsd24Hr,
    decimal ChangePercent24Hr
) : IRequest<Result>;
