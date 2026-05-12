# Diseño Técnico: Capa de Consulta y Dashboard de Trazabilidad

## Objetivo

Implementar una capa de consulta de trazabilidad desacoplada del motor de almacenamiento, orientada a dashboards y exploración funcional, donde:

- SQL sigue siendo la persistencia durable y oficial de auditoría.
- MongoDB actúa como read model para consultas, filtros, agregaciones y vistas recompuestas.
- El frontend consume una API HTTP estable del backend y no se conecta directo a MongoDB.
- La misma interfaz de consulta puede migrar en el futuro a otros read stores como vistas materializadas, Elasticsearch o Redis como cache complementaria.

## Estado Actual

### Persistencia durable

La trazabilidad durable hoy queda en SQL sobre `AuditOutboxMessages` y `AuditOutboxChanges`, generada desde el contexto:

- [AUT2ServicesContext.cs](E:/DST/dev/AUT2Services/src/AUT2Services.Infra.Data/AI_Context/AUT2ServicesContext.cs)
- [AuditOutboxMessage.cs](E:/DST/dev/AUT2Services/src/AUT2Services.Infra.DataTrazabilidad/Persistence/AuditOutboxMessage.cs)

### Lectura actual

La lectura actual del journal solo existe por `AggregateId`, sin filtros transversales:

- [IAuditJournalReader.cs](E:/DST/dev/AUT2Services/src/AUT2Services.Domain.Core/Auditing/IAuditJournalReader.cs)
- [EfAuditJournalReader.cs](E:/DST/dev/AUT2Services/src/AUT2Services.Infra.Data/Auditing/EfAuditJournalReader.cs)

### Proyección Mongo

Mongo hoy recibe una proyección opcional del outbox SQL:

- [MongoAuditProjectionWriter.cs](E:/DST/dev/AUT2Services/src/AUT2Services.Infra.DataMongoDB/Services/MongoAuditProjectionWriter.cs)
- [AuditOutboxMongoProjectionService.cs](E:/DST/dev/AUT2Services/src/AUT2Services.Infra.DataMongoDB/Services/AuditOutboxMongoProjectionService.cs)
- [MongoAuditTimelineDocument.cs](E:/DST/dev/AUT2Services/src/AUT2Services.Infra.DataMongoDB/Models/MongoAuditTimelineDocument.cs)

### Catálogo real de entidades trazables

El `AggregateType` persistido hoy corresponde al nombre CLR del agregado:

- [InMemoryAuditBuffer.cs](E:/DST/dev/AUT2Services/src/AUT2Services.Infra.DataTrazabilidad/Auditing/InMemoryAuditBuffer.cs)

Por lo tanto, para la primera fase, las claves reales del catálogo pueden ser:

- `Entidad`
- `Organizacion`
- `UnidadOrganizacional`
- `Proveedor`
- `Proceso`
- `PoliticaAsignada`
- `ValidacionEnrrolamiento`
- `AutenticadorExterno`

## Decisión Arquitectónica

### Decisión principal

Se implementará una **API transversal de trazabilidad** soportada por una **capa de query abstraída por provider**.

### Qué no hacer

- no exponer MongoDB directamente al frontend
- no acoplar el dashboard a la colección `audit_trail`
- no duplicar endpoints `BuscarTrazabilidadPor_Id` por entidad como base del dashboard

### Qué sí hacer

- mantener SQL como verdad durable
- usar MongoDB como read model principal para dashboards
- mantener fallback a SQL cuando Mongo esté deshabilitado o no disponible
- definir DTOs y contratos HTTP propios para trazabilidad

## Flujo de Consulta Propuesto

1. El frontend solicita entidades consultables.
2. El usuario selecciona `Entity` y `AggregateId`, más filtros opcionales.
3. El controller de trazabilidad delega en un servicio de query.
4. El servicio elige provider de lectura:
   - `mongo`
   - `sql`
   - `auto`
5. El provider devuelve resultados paginados y/o detalle del evento.
6. El frontend dibuja tabla, timeline y panel de detalle.

## Diseño Backend

### Estructura de carpetas propuesta

#### API

```text
src/AUT2Services.Services.API/
  Controllers/
    TraceabilityController.cs
```

#### Application o Core de consulta

```text
src/AUT2Services.Domain.Core/Auditing/Queries/
  ITraceabilityQueryService.cs
  ITraceabilityReadStore.cs
  ITraceabilityEntityCatalog.cs
  TraceabilityReadProvider.cs
  TraceabilityQueryOptions.cs
```

#### DTOs de salida y filtros

```text
src/AUT2Services.Domain.Core/Auditing/Queries/Models/
  TraceabilityEntityCatalogItem.cs
  TraceabilitySearchRequest.cs
  TraceabilitySearchResponse.cs
  TraceabilityEventListItem.cs
  TraceabilityEventDetail.cs
  TraceabilityChangeItem.cs
  TraceabilityFilterOptions.cs
  TraceabilityFilterValuesRequest.cs
  TraceabilityAggregateContext.cs
  PagedResult.cs
```

#### Implementación SQL

```text
src/AUT2Services.Infra.Data/Auditing/Queries/
  SqlTraceabilityReadStore.cs
  StaticTraceabilityEntityCatalog.cs
```

#### Implementación Mongo

```text
src/AUT2Services.Infra.DataMongoDB/Queries/
  MongoTraceabilityReadStore.cs
  MongoTraceabilityIndexInitializer.cs
```

#### Orquestación

```text
src/AUT2Services.Infra.Cross.IoC/
  TraceabilityNativeInjectorBootStrapper.cs
```

## Interfaces Propuestas

### 1. Servicio de aplicación para el controller

```csharp
public interface ITraceabilityQueryService
{
    Task<IReadOnlyList<TraceabilityEntityCatalogItem>> GetEntitiesAsync(CancellationToken cancellationToken = default);

    Task<TraceabilityFilterOptions> GetFilterOptionsAsync(
        TraceabilityFilterValuesRequest request,
        CancellationToken cancellationToken = default);

    Task<PagedResult<TraceabilityEventListItem>> SearchAsync(
        TraceabilitySearchRequest request,
        CancellationToken cancellationToken = default);

    Task<TraceabilityEventDetail?> GetEventAsync(
        Guid eventId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TraceabilityEventListItem>> GetAggregateTimelineAsync(
        string entityKey,
        Guid aggregateId,
        DateTimeOffset? fromUtc = null,
        DateTimeOffset? toUtc = null,
        CancellationToken cancellationToken = default);
}
```

### 2. Abstracción por provider

```csharp
public interface ITraceabilityReadStore
{
    TraceabilityReadProvider Provider { get; }

    Task<PagedResult<TraceabilityEventListItem>> SearchAsync(
        TraceabilitySearchRequest request,
        CancellationToken cancellationToken = default);

    Task<TraceabilityEventDetail?> GetEventAsync(
        Guid eventId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TraceabilityEventListItem>> GetAggregateTimelineAsync(
        string entityKey,
        Guid aggregateId,
        DateTimeOffset? fromUtc,
        DateTimeOffset? toUtc,
        CancellationToken cancellationToken = default);

    Task<TraceabilityFilterOptions> GetFilterOptionsAsync(
        TraceabilityFilterValuesRequest request,
        CancellationToken cancellationToken = default);
}
```

### 3. Catálogo de entidades consultables

```csharp
public interface ITraceabilityEntityCatalog
{
    IReadOnlyList<TraceabilityEntityCatalogItem> GetAll();
    TraceabilityEntityCatalogItem? Find(string entityKey);
}
```

## Enumeración de provider

```csharp
public enum TraceabilityReadProvider
{
    Auto = 0,
    Sql = 1,
    Mongo = 2
}
```

## DTOs Propuestos

### 1. Catálogo de entidades

```csharp
public sealed class TraceabilityEntityCatalogItem
{
    public string EntityKey { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string AggregateType { get; init; } = string.Empty;
    public bool Enabled { get; init; }
}
```

Valores iniciales sugeridos:

| EntityKey | DisplayName | AggregateType |
|---|---|---|
| `entidad` | `Entidad` | `Entidad` |
| `organizacion` | `Organización` | `Organizacion` |
| `unidad-organizacional` | `Unidad Organizacional` | `UnidadOrganizacional` |
| `proveedor` | `Proveedor` | `Proveedor` |
| `proceso` | `Proceso` | `Proceso` |
| `politica-asignada` | `Política Asignada` | `PoliticaAsignada` |
| `validacion-enrrolamiento` | `Validación Enrrolamiento` | `ValidacionEnrrolamiento` |
| `autenticador-externo` | `Autenticador Externo` | `AutenticadorExterno` |

### 2. Request principal de búsqueda

```csharp
public sealed class TraceabilitySearchRequest
{
    public string? EntityKey { get; init; }
    public Guid? AggregateId { get; init; }
    public DateTimeOffset? FromUtc { get; init; }
    public DateTimeOffset? ToUtc { get; init; }
    public IReadOnlyCollection<string> OperationTypes { get; init; } = [];
    public IReadOnlyCollection<string> EventTypes { get; init; } = [];
    public IReadOnlyCollection<string> CommandTypes { get; init; } = [];
    public Guid? CorrelationId { get; init; }
    public string? ActorUserId { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;
    public string SortDirection { get; init; } = "desc";
}
```

### 3. Ítem de lista

```csharp
public sealed class TraceabilityEventListItem
{
    public Guid Id { get; init; }
    public Guid AggregateId { get; init; }
    public string AggregateType { get; init; } = string.Empty;
    public long AggregateRevision { get; init; }
    public string EventType { get; init; } = string.Empty;
    public string CommandType { get; init; } = string.Empty;
    public string OperationType { get; init; } = string.Empty;
    public DateTimeOffset OccurredAtUtc { get; init; }
    public DateTimeOffset PersistedAtUtc { get; init; }
    public string? ActorUsername { get; init; }
    public string? ActorEmail { get; init; }
    public string? RequestPath { get; init; }
    public int ChangeCount { get; init; }
}
```

### 4. Detalle de evento

```csharp
public sealed class TraceabilityEventDetail
{
    public Guid Id { get; init; }
    public Guid CorrelationId { get; init; }
    public Guid AggregateId { get; init; }
    public string AggregateType { get; init; } = string.Empty;
    public long AggregateRevision { get; init; }
    public string EventType { get; init; } = string.Empty;
    public string CommandType { get; init; } = string.Empty;
    public string OperationType { get; init; } = string.Empty;
    public DateTimeOffset OccurredAtUtc { get; init; }
    public DateTimeOffset PersistedAtUtc { get; init; }
    public string? RequestPath { get; init; }
    public string? SnapshotJson { get; init; }
    public TraceabilityActor Actor { get; init; } = new();
    public IReadOnlyCollection<TraceabilityChangeItem> Changes { get; init; } = [];
}
```

### 5. Cambio individual

```csharp
public sealed class TraceabilityChangeItem
{
    public int Order { get; init; }
    public string Path { get; init; } = string.Empty;
    public string ValueType { get; init; } = string.Empty;
    public string? NewValueJson { get; init; }
}
```

### 6. Valores de filtros

```csharp
public sealed class TraceabilityFilterValuesRequest
{
    public string? EntityKey { get; init; }
    public Guid? AggregateId { get; init; }
    public DateTimeOffset? FromUtc { get; init; }
    public DateTimeOffset? ToUtc { get; init; }
}
```

```csharp
public sealed class TraceabilityFilterOptions
{
    public IReadOnlyCollection<string> OperationTypes { get; init; } = [];
    public IReadOnlyCollection<string> EventTypes { get; init; } = [];
    public IReadOnlyCollection<string> CommandTypes { get; init; } = [];
}
```

## Endpoints Propuestos

### 1. Catálogo de entidades

```http
GET /api/traceability/entities
```

Response:

```json
[
  {
    "entityKey": "entidad",
    "displayName": "Entidad",
    "aggregateType": "Entidad",
    "enabled": true
  }
]
```

### 2. Filtros disponibles

```http
POST /api/traceability/filter-options
```

Request:

```json
{
  "entityKey": "entidad",
  "aggregateId": "15507441-5792-4c46-9334-9a5faa99e204",
  "fromUtc": "2026-04-01T00:00:00Z",
  "toUtc": "2026-04-30T23:59:59Z"
}
```

### 3. Búsqueda principal

```http
POST /api/traceability/search
```

Request:

```json
{
  "entityKey": "entidad",
  "aggregateId": "15507441-5792-4c46-9334-9a5faa99e204",
  "fromUtc": "2026-04-01T00:00:00Z",
  "toUtc": "2026-04-30T23:59:59Z",
  "operationTypes": ["Update"],
  "eventTypes": ["EntidadEventModificado"],
  "commandTypes": ["ModificarEntidadCommand"],
  "page": 1,
  "pageSize": 50,
  "sortDirection": "desc"
}
```

### 4. Timeline rápida por agregado

```http
GET /api/traceability/aggregate/{aggregateId}?entityKey=entidad&fromUtc=...&toUtc=...
```

Uso principal:

- abrir el dashboard desde otra pantalla ya contextualizado
- cargar la historia de un agregado sin pasar por búsqueda manual

### 5. Detalle de evento

```http
GET /api/traceability/events/{id}
```

## Controller Propuesto

Archivo:

```text
src/AUT2Services.Services.API/Controllers/TraceabilityController.cs
```

Contrato sugerido:

```csharp
[Route("api/[controller]")]
[Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD)]
public sealed class TraceabilityController : ControllerBase
{
    [HttpGet("entities")]
    public Task<ActionResult<IReadOnlyList<TraceabilityEntityCatalogItem>>> GetEntities()

    [HttpPost("filter-options")]
    public Task<ActionResult<TraceabilityFilterOptions>> GetFilterOptions([FromBody] TraceabilityFilterValuesRequest request)

    [HttpPost("search")]
    public Task<ActionResult<PagedResult<TraceabilityEventListItem>>> Search([FromBody] TraceabilitySearchRequest request)

    [HttpGet("aggregate/{aggregateId:guid}")]
    public Task<ActionResult<IReadOnlyList<TraceabilityEventListItem>>> GetAggregateTimeline(
        Guid aggregateId,
        [FromQuery] string entityKey,
        [FromQuery] DateTimeOffset? fromUtc,
        [FromQuery] DateTimeOffset? toUtc)

    [HttpGet("events/{id:guid}")]
    public Task<ActionResult<TraceabilityEventDetail>> GetEvent(Guid id)
}
```

## Estrategia de Provider

### Opción recomendada

Configurar un selector por opciones:

```json
"TraceabilityRead": {
  "PreferredProvider": "mongo",
  "FallbackToSql": true
}
```

### Reglas

- `mongo`: intenta Mongo primero
- si Mongo no está disponible y `FallbackToSql = true`, consulta SQL
- `sql`: consulta siempre SQL
- `auto`: usa Mongo cuando está habilitado y saludable, si no SQL

## Diseño Mongo para consultas

### Primera fase

Se puede consultar la colección actual `audit_trail`, porque ya contiene:

- `AggregateId`
- `AggregateType`
- `AggregateRevision`
- `EventType`
- `CommandType`
- `OperationType`
- `OccurredAtUtc`
- `PersistedAtUtc`
- `Actor`
- `RequestPath`
- `Changes`

### Índices recomendados

- `{ AggregateId: 1, AggregateRevision: 1 }`
- `{ AggregateType: 1, OccurredAtUtc: -1 }`
- `{ EventType: 1, OccurredAtUtc: -1 }`
- `{ CommandType: 1, OccurredAtUtc: -1 }`
- `{ OperationType: 1, OccurredAtUtc: -1 }`
- `{ CorrelationId: 1 }`
- `{ PersistedAtUtc: -1 }`

### Segunda fase recomendada

Además del espejo de eventos, crear colecciones optimizadas:

- `audit_events`
- `audit_filter_values`
- `audit_aggregate_summary`

Esto permite:

- catálogos rápidos
- filtros preagregados
- dashboards más pesados sin recalcular todo

## Diseño Frontend

### Estructura de carpetas propuesta

```text
src/app/core/traceability/
  traceability.models.ts
  traceability-query-params.ts
  traceability.service.ts

src/app/paneles/trazabilidad-dashboard/
  trazabilidad-dashboard.component.ts
  trazabilidad-dashboard.component.html
  trazabilidad-dashboard.component.scss
  components/
    traceability-filters.component.ts
    traceability-filters.component.html
    traceability-results-table.component.ts
    traceability-results-table.component.html
    traceability-event-detail.component.ts
    traceability-event-detail.component.html
```

### Ruta propuesta

Archivo a modificar:

- [paneles.routes.ts](E:/DST/dev/AUT2/src/app/paneles/paneles.routes.ts)

Ruta:

```ts
{
  path: 'trazabilidad',
  component: TrazabilidadDashboardComponent,
  canActivate: [authenticatedGuard],
}
```

### Query params para abrir desde otra pantalla

```text
/paneles/trazabilidad?entity=entidad&aggregateId=<guid>&from=2026-04-01&to=2026-04-30
```

### Modelo frontend sugerido

```ts
export interface TraceabilitySearchForm {
  entityKey: string | null;
  aggregateId: string | null;
  fromUtc: string | null;
  toUtc: string | null;
  operationTypes: string[];
  eventTypes: string[];
  commandTypes: string[];
  page: number;
  pageSize: number;
  sortDirection: 'asc' | 'desc';
}
```

### Componentes mínimos

1. `TraceabilityFiltersComponent`
   - selector entidad
   - input aggregateId
   - fecha inicio
   - fecha término
   - multi-select de operation types
   - multi-select de event types
   - multi-select de command types

2. `TraceabilityResultsTableComponent`
   - `ngx-datatable`
   - columnas:
     - fecha
     - operación
     - command
     - event
     - revisión
     - actor
     - requestPath

3. `TraceabilityEventDetailComponent`
   - detalle del evento seleccionado
   - lista de cambios
   - snapshot json

## Integración desde otras pantallas

### Patrón recomendado

Agregar acción “Ver trazabilidad” donde ya exista contexto de entidad/agregado.

Ejemplo:

```ts
this.router.navigate(['/paneles/trazabilidad'], {
  queryParams: {
    entity: 'entidad',
    aggregateId: row.id,
  },
});
```

## Seguridad

### Recomendación

Primera fase:

- proteger el controller con policy administrativa

Segunda fase:

- política específica, por ejemplo `TRAZABILIDAD_LECTURA`

### Consideración importante

No devolver por defecto payloads masivos como `SnapshotJson` en la búsqueda principal. El detalle debe cargarse por evento para reducir exposición y volumen.

## Orden de Implementación

1. Crear DTOs y contratos en `Domain.Core/Auditing/Queries`.
2. Crear `ITraceabilityEntityCatalog`.
3. Crear `SqlTraceabilityReadStore`.
4. Crear `MongoTraceabilityReadStore`.
5. Crear `TraceabilityQueryService` con selector de provider.
6. Registrar dependencias en IoC.
7. Exponer `TraceabilityController`.
8. Crear servicio frontend y modelos.
9. Crear dashboard y ruta.
10. Integrar apertura contextual desde otras pantallas.

## Entregable mínimo viable

### Backend MVP

- `GET /api/traceability/entities`
- `POST /api/traceability/search`
- `GET /api/traceability/events/{id}`
- provider SQL funcional
- provider Mongo opcional

### Frontend MVP

- selector entidad
- input aggregateId
- fechas desde/hasta
- filtro operationType
- tabla de resultados
- panel de detalle
- soporte de query params

## Decisión final

La capa de consulta debe implementarse sobre una API transversal propia de trazabilidad, con provider de lectura seleccionable y MongoDB como read model principal de dashboards, manteniendo SQL como respaldo durable y fallback operativo.
