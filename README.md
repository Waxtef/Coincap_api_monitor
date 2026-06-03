# CoinCap Monitor

Servicio backend para monitoreo de activos digitales en tiempo real. Se integra con la API de CoinCap para consultar, almacenar y analizar criptomonedas, con detección automática de variaciones de precio superiores al 5%.

## Tech Stack

- **.NET 10** — Web API
- **SQLite + EF Core 10** — Base de datos local
- **MediatR** — CQRS + Domain Events
- **Polly / Microsoft.Extensions.Http.Resilience** — resiliencia HTTP
- **Serilog** — logging estructurado
- **Swagger** — documentación interactiva de la API
- **xUnit + Moq** — tests unitarios

## Prerrequisitos

| Herramienta | Versión |
|---|---|
| .NET SDK | 10.0 |
| Visual Studio mas Actual | o cualquier IDE con soporte .NET |

## Instalación

```bash
# 1. Clonar el repositorio
git clone https://github.com/Waxtef/Coincap_api_monitor.git
cd coincap_api_monitor

# 2. Restaurar dependencias
dotnet restore

# 3. Configurar la ApiKey de CoinCap (registro en pro.coincap.io)
cd src/Coincap.API
dotnet user-secrets init
dotnet user-secrets set "CoinCap:ApiKey" "TU_API_KEY_AQUI"

# 4. Aplicar migraciones (crea coincap.db automáticamente)
# Desde el Package Manager Console en Visual Studio:
Update-Database -Project Coincap.API -StartupProject Coincap.API
```

## Cómo correr

```bash
dotnet run --project src/Coincap.API
```

La aplicación arranca por ejemplo en `https://localhost:7018`. Al iniciar sincroniza automáticamente los datos desde CoinCap.

Abre el navegador en `https://localhost:7018/swagger` para usar el Api Backend.

## Endpoints

| Método | Ruta | Descripción |
|---|---|---|
| `GET` | `/api/assets` | Listado paginado de criptomonedas.|
| `GET` | `/api/assets/{id}` | Detalle de un activo por Id`bitcoin`. |
| `GET` | `/api/assets/{id}/history` | Histórico de precios. |
| `POST` | `/api/sync` | Sincronización manual con CoinCap. |
| `GET` | `/api/alerts` | Activos con variaciones de precio superiores al 5%.|

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


## Tests

```bash
# Todos los tests
dotnet test

# Solo tests de alertas
dotnet test tests/Coincap.Alerts.Tests

# Solo tests de assets
dotnet test tests/Coincap.Assets.Tests
```
