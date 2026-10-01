# Trazabilidad de auditoría y proyección MongoDB

## Propósito y alcance

Este documento describe la implementación observada de trazabilidad en AUT2Services. Cubre cambios de negocio de Administración, operaciones relevantes de `AUT2Services.Infra.Security` y su proyección asíncrona hacia MongoDB.

No describe credenciales, URI, nombres de servidores ni procedimientos operacionales para consultar o modificar bases de datos.

La auditoría tiene dos persistencias con responsabilidades distintas:

1. `AuditOutbox` y `AuditOutboxChange` son el registro durable y transaccional en la base de datos relacional de AUT2Services.
2. MongoDB recibe una proyección de esos registros para la colección de timeline de auditoría.

MongoDB no es la fuente primaria que determina si una operación de negocio fue confirmada.

## Componentes principales

| Responsabilidad | Proyecto | Símbolo principal |
|---|---|---|
| Captura transitoria de eventos auditables | `AUT2Services.Infra.DataTrazabilidad` | `InMemoryAuditBuffer` |
| Contrato y helpers de captura | `AUT2Services.Domain.Core` | `IAuditBuffer`, `CommandHandler` |
| Confirmación relacional de outbox | `AUT2Services.Infra.Data` | `AUT2ServicesContext` |
| Modelo relacional de auditoría | `AUT2Services.Infra.DataTrazabilidad` | `AuditOutboxMessage`, `AuditOutboxChange`, `AuditAggregateCursor` |
| Proyección a MongoDB | `AUT2Services.Infra.DataMongoDB` | `AuditOutboxMongoProjectionService`, `MongoAuditProjectionWriter` |
| Documento MongoDB | `AUT2Services.Infra.DataMongoDB` | `MongoAuditTimelineDocument` |
| Contexto de actor y solicitud HTTP | `AUT2Services.Services.API` | `AuditExecutionContextMiddleware` |
| Captura especializada de identidad | `AUT2Services.Infra.Security` | `SecurityTraceabilityService` |

El registro de dependencias se inicia desde `AUT2Services.Services.API/Program.cs`, que agrega la proyección mediante `AddMongoAuditProjection(...)` y el middleware de contexto de auditoría.

## Flujo base: cambio de una entidad de Administración

Los handlers de comandos de las entidades de Administración derivan de `CommandHandler`. Para crear, actualizar o eliminar, usan respectivamente `AddCreateDomainEvent`, `AddUpdateDomainEvent` o `AddDeleteDomainEvent`.

```text
Solicitud HTTP autenticada
  -> AuditExecutionContextMiddleware identifica correlación, actor y ruta
  -> Controller / servicio de aplicación envía un comando
  -> Handler de dominio modifica el agregado
  -> Add*DomainEvent agrega evento de dominio e IAuditBuffer registra antes/después
  -> Repository / UnitOfWork invoca AUT2ServicesContext.Commit()
  -> transacción relacional persiste cambio de negocio + AuditOutbox + AuditOutboxChange
  -> respuesta funcional de la operación
  -> worker en segundo plano proyecta el lote pendiente a MongoDB
```

### Ejemplo: modificación de Proceso

`Domain/AI_Commands/Procesos/Handlers/ModificarProcesoHandler.cs` construye el estado nuevo de `Proceso`, registra `ProcesoEventModificado` mediante `AddUpdateDomainEvent(...)` y luego confirma el `UnitOfWork`.

La entrada de auditoría contiene:

- el tipo de agregado (`Proceso`);
- el tipo de comando y evento;
- identificador del agregado, correlación, actor y ruta de la solicitud;
- operación `Update`;
- lista de propiedades modificadas, calculada desde el estado previo y posterior;
- snapshot cuando el comando lo solicita.

El mismo patrón se observa para Entidad, Organización, Unidad Organizacional, Proveedor, Política Asignada, Validación de Enrolamiento y Autenticador Externo. El catálogo de trazabilidad habilitado incorpora además Usuario.

## Persistencia relacional de la outbox

`AUT2ServicesContext.Commit()` drena `IAuditBuffer` y prepara los registros antes de ejecutar `SaveChangesAsync`.

```text
IAuditBuffer.Drain()
  -> PrepareAuditOutboxMessagesAsync(...)
  -> AuditAggregateCursor asigna revisión consecutiva por agregado
  -> AuditOutboxMessage registra metadatos y snapshot
  -> AuditOutboxChange registra cada diferencia de actualización
  -> SaveChangesAsync confirma junto con el cambio de negocio
```

`AuditOutboxMessage` conserva el estado de despacho:

- `DispatchStatus`: pendiente o procesado;
- `DispatchAttempts` y `LastDispatchAttemptUtc`;
- `DispatchedAtUtc`;
- `LastError`, si la proyección falla.

La revisión por agregado se mantiene en `AuditAggregateCursor`; además existe una restricción única para `(AggregateId, AggregateRevision)`. Esto evita que la timeline pierda el orden lógico de los eventos de un mismo agregado.

## Flujo de proyección hacia MongoDB

`AuditOutboxMongoProjectionService` es un `BackgroundService`. Si la proyección está deshabilitada o su configuración esencial no está disponible, el servicio se detiene sin intentar procesar la outbox.

```text
Cada IntervalSeconds
  -> obtiene hasta BatchSize mensajes AuditOutbox pendientes, ordenados por persistencia
  -> incluye sus AuditOutboxChange
  -> los convierte a AuditEnvelope
  -> IAuditProjectionWriter.WriteAsync(...)
  -> MongoAuditProjectionWriter construye MongoAuditTimelineDocument
  -> BulkWrite con ReplaceOne + upsert por Id de auditoría
  -> éxito: marca el lote procesado y limpia LastError
  -> excepción: guarda intento y LastError; el estado queda pendiente para reintento
```

`MongoAuditProjectionWriter` es la única clase productiva que crea `MongoClient` y escribe la colección configurada. El uso de `ReplaceOne` con `IsUpsert = true` hace idempotente la proyección de un mismo identificador de auditoría.

`MongoAuditTimelineDocument` proyecta los identificadores `Id`, `CorrelationId` y `AggregateId` con `GuidRepresentation.Standard`, además de los datos de evento, actor, snapshot y cambios. Los documentos de cambio incluyen orden, ruta de propiedad, tipo de valor y valor nuevo serializado.

## Flujo de identidad y autenticación

`AUT2Services.Infra.Security` no escribe directamente a MongoDB y no referencia el proyecto `AUT2Services.Infra.DataMongoDB`. Registra eventos a través de `SecurityTraceabilityService`, que usa el mismo `IAuditBuffer` y por tanto la misma outbox relacional y proyector MongoDB.

```text
Handler de cuenta o autenticación
  -> SecurityTraceabilityService.TrackCreate / TrackUpdate
  -> IAuditBuffer
  -> AUT2ServicesContext.Commit() dentro de la transacción del handler
  -> AuditOutbox relacional
  -> proyección asíncrona MongoDB
```

Casos observados:

- alta de usuario, incluida la creación de un usuario desde ClaveÚnica;
- modificación, activación y desactivación de usuario;
- cambios y confirmaciones de correo;
- solicitud, respuesta y confirmación de flujos de cambio o recuperación de clave;
- programación de notificaciones de seguridad relevantes.

En `LoginClaveUnicaCommandHandler`, la trazabilidad de registro se agrega cuando el usuario es creado. El login exitoso de un usuario ya existente no agrega, en el flujo inspeccionado, un evento equivalente de inicio de sesión.

`NotificationOutbox` también aparece en `Infra.Security`, pero es un mecanismo distinto: despacha notificaciones de correo o Zendesk desde la persistencia relacional. No es la proyección de auditoría MongoDB.

## Lectura de trazabilidad en la API

`TraceabilityController` expone la búsqueda, detalle y timeline de trazabilidad. La implementación actual usa `SqlTraceabilityReadStore` y `EfAuditJournalReader`, ambos consultan `AuditOutboxMessages` y sus cambios desde la base relacional.

Por ello:

- la vista actual de trazabilidad no depende de que MongoDB esté disponible;
- MongoDB es una proyección secundaria y puede presentar retraso respecto del registro relacional;
- un fallo de MongoDB debe revisarse mediante el estado de despacho de la outbox, no asumiendo que se perdió el cambio de negocio.

## Datos auditados y consideración de seguridad

El constructor de deltas `DefaultAuditDeltaBuilder` inspecciona las propiedades públicas del objeto actualizado y serializa los valores nuevos de cada propiedad distinta; solo excluye `DomainEvents`.

Actualmente no existe una lista de exclusión ni enmascaramiento por nombre de propiedad en ese componente. Como consecuencia, si una actualización de `Proceso` modifica la propiedad `Token`, el delta puede persistir el nuevo valor serializado en `AuditOutboxChange` y luego proyectarlo a MongoDB. En el flujo de seguridad, `UsuarioTraceabilityState` también dispone de propiedades de validación que pueden transportar códigos o tokens; el mismo mecanismo de serialización requiere protegerlas.

Este comportamiento es un riesgo de exposición de secretos. Una corrección futura debe aplicar una política centralizada de redacción o exclusión antes de persistir el delta, conservando el hecho de que la propiedad cambió pero sin almacenar su valor sensible.

## Condiciones para que una operación deje trazabilidad

Una operación debe cumplir todas estas condiciones:

1. registrar un `AddCreateDomainEvent`, `AddUpdateDomainEvent`, `AddDeleteDomainEvent` o una llamada equivalente a `SecurityTraceabilityService`;
2. alcanzar `AUT2ServicesContext.Commit()` exitosamente;
3. tener habilitada la proyección MongoDB y una configuración operativa;
4. permitir que el worker procese el mensaje pendiente sin error.

Si falla el paso 1, no se genera registro de auditoría. Si falla el paso 2, la transacción no se confirma. Si falla el paso 3 o 4, el registro relacional permanece como fuente de trazabilidad y la proyección podrá quedar pendiente para reintento.

## Pruebas existentes

El proyecto `AUT2Services.Tests` contiene pruebas de integración de auditoría bajo `Integration/Auditing`, incluidas pruebas para el escritor MongoDB y el worker de proyección. Estas pruebas requieren una infraestructura local controlada; no se ejecutan como parte de este documento.

## Referencias de código

- `AUT2Services.Services.API/Program.cs`
- `AUT2Services.Services.API/Configurations/AuditExecutionContextMiddleware.cs`
- `AUT2Services.Domain.Core/Commands/CommandHandler.cs`
- `AUT2Services.Infra.DataTrazabilidad/Auditing/InMemoryAuditBuffer.cs`
- `AUT2Services.Infra.Data/AI_Context/AUT2ServicesContext.cs`
- `AUT2Services.Infra.DataMongoDB/Services/AuditOutboxMongoProjectionService.cs`
- `AUT2Services.Infra.DataMongoDB/Services/MongoAuditProjectionWriter.cs`
- `AUT2Services.Infra.DataMongoDB/Models/MongoAuditTimelineDocument.cs`
- `AUT2Services.Infra.Security/Services/SecurityTraceabilityService.cs`
- `AUT2Services.Infra.Data/Auditing/Queries/SqlTraceabilityReadStore.cs`
