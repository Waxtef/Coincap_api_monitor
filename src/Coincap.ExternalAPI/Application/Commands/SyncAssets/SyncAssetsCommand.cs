using Coincap.Share;
using MediatR;

namespace Coincap.ExternalAPI.Application.Commands.SyncAssets;

// Command que dispara la conexion con CoinCap
// Lo puede enviar manual o automático
public record SyncAssetsCommand : IRequest<SyncAssetsResult>;

// Resultado de cantidad activos y cuando se tomaron
public record SyncAssetsResult(int AssetsProcessed, DateTime SyncedAt);
