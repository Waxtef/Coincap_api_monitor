# CoinCap Monitor

Servicio backend para monitoreo de activos digitales en tiempo real. Se integra con la API de CoinCap para consultar, almacenar y analizar criptomonedas, con detección automática de variaciones de precio superiores al 5%.

## Tech Stack

- **.NET 10** — Web API
- **SQLite + EF Core 10** — persistencia local
- **MediatR** — CQRS + Domain Events
- **Polly / Microsoft.Extensions.Http.Resilience** — resiliencia HTTP
- **Serilog** — logging estructurado
- **Swagger** — documentación interactiva de la API
- **xUnit + Moq** — tests unitarios

## Prerrequisitos

| Herramienta | Versión |
|---|---|
| .NET SDK | 10.0 |
| Git | cualquier versión reciente |
| Visual Studio 2022+ | o cualquier IDE con soporte .NET |

## Instalación

```bash
# 1. Clonar el repositorio
git clone https://github.com/TU_USUARIO/coincap-monitor.git
cd coincap-monitor

# 2. Restaurar dependencias
dotnet restore

# 3. Configurar la ApiKey de CoinCap (registro gratuito en pro.coincap.io)
cd src/Coincap.API
dotnet user-secrets init
dotnet user-secrets set "CoinCap:ApiKey" "TU_API_KEY_AQUI"
cd ../..

# 4. Aplicar migraciones (crea coincap.db automáticamente)
dotnet ef database update --project src/Coincap.API
```

## Cómo correr

```bash
dotnet run --project src/Coincap.API
```

La aplicación arranca en `https://localhost:7018`. Al iniciar sincroniza automáticamente los datos desde CoinCap.

Abre el navegador en `https://localhost:7018/swagger` para ver la documentación interactiva.

## Endpoints

| Método | Ruta | Descripción |
|---|---|---|
| `GET` | `/api/assets` | Listado paginado de criptomonedas. Params: `?page=1&pageSize=20&search=bitcoin` |
| `GET` | `/api/assets/{id}` | Detalle de un activo por Id (ej: `bitcoin`) |
| `GET` | `/api/assets/{id}/history` | Histórico de precios. Params: `?from=2024-01-01&to=2024-12-31` |
| `POST` | `/api/sync` | Sincronización manual con CoinCap |
| `GET` | `/api/alerts` | Activos con variaciones de precio superiores al 5%. Param: `?assetId=bitcoin` |

## Configuración

Todos los parámetros operacionales se configuran en `appsettings.json`:

```json
"CoinCap": {
  "ApiKey": "",
  "BaseUrl": "https://rest.coincap.io/v3",
  "SyncIntervalMinutes": 60,
  "AlertThresholdPercent": 5.0,
  "AssetLimit": 100
}
```

> ⚠️ Nunca subas tu ApiKey al repositorio. Usa User Secrets en desarrollo.

## Tests

```bash
# Todos los tests
dotnet test

# Solo tests de alertas
dotnet test tests/Coincap.Alerts.Tests

# Solo tests de assets
dotnet test tests/Coincap.Assets.Tests
```
