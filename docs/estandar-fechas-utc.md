# Estandar Tecnico de Fechas y Timestamps UTC

## Objetivo

Este documento define el estandar obligatorio para modelar, persistir, comparar y exponer fechas en AUT2Services.

El objetivo es:

- operar solo en UTC para todo instante real en el tiempo;
- mantener compatibilidad entre PostgreSQL y SQL Server;
- eliminar ambiguedades entre hora local, UTC y offsets;
- reducir errores de negocio, serializacion, persistencia y migraciones.

## Alcance

Este estandar aplica a:

- dominio;
- application;
- infra data;
- infra security;
- servicios API;
- migraciones;
- seeds;
- serializacion JSON;
- validaciones de entrada y salida.

## Principios

1. Todo instante real en el tiempo se representa en UTC.
2. La zona horaria no es un dato de negocio dentro del dominio actual.
3. La aplicacion no usa hora local del servidor.
4. Un mismo concepto temporal no puede coexistir con tipos distintos.
5. Fechas de calendario y timestamps no se mezclan.

## Tipos Permitidos

### 1. Instantes reales

Se usa `DateTimeOffset` con offset `+00:00`.

Ejemplos:

- fecha de creacion;
- fecha de actualizacion;
- fecha de expiracion;
- fecha de revocacion;
- fecha de validacion efectiva;
- timestamp de respuesta o evento.

### 2. Fechas de calendario

Se usa `DateOnly`.

Ejemplos:

- fecha de nacimiento;
- dia de inicio sin componente horario;
- dia de cierre sin componente horario.

### 3. Duraciones o ventanas de tiempo

Se usa `TimeSpan`.

Ejemplos:

- expiracion en minutos;
- ventana de rate limit;
- intervalo de limpieza.

## Tipos No Permitidos

- `DateTime.Now`
- `DateTimeOffset.Now`
- `DateTime` para representar instantes de negocio persistidos
- mezclar `DateTime` y `DateTimeOffset` para el mismo concepto
- `DateTimeOffset.MinValue`
- `DateTimeOffset.MaxValue`
- `DateTime.MinValue`
- `DateTime.MaxValue`
- offsets distintos de `00:00` en datos persistidos

## Reglas Obligatorias de Modelado

### Regla 1. Instantes persistidos

Todo campo persistido que represente un instante debe ser `DateTimeOffset`.

Nombres recomendados:

- `CreatedAtUtc`
- `UpdatedAtUtc`
- `ExpiresAtUtc`
- `RevokedAtUtc`
- `IssuedAtUtc`
- `LastLoginAtUtc`

Si el nombre historico ya existe, se puede mantener temporalmente, pero el tipo y el comportamiento deben seguir este estandar.

### Regla 2. Calendario sin hora

Todo dato que no representa un instante sino una fecha humana debe ser `DateOnly`.

Ejemplo:

- `FechaDeNacimiento`

No se debe guardar como `DateTime` ni como `DateTimeOffset`.

### Regla 3. UTC como unica fuente de tiempo

El codigo no debe invocar `UtcNow` directamente desde cualquier clase de negocio o infraestructura. Debe usar un reloj abstraido.

Interfaz base recomendada:

```csharp
public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
```

Implementacion por defecto:

```csharp
public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
```

### Regla 4. Sin sentinelas

No se usan `MinValue` ni `MaxValue` para modelar ausencia de fecha o vigencia indefinida.

Se debe usar una de estas dos estrategias:

- `null` para fecha ausente;
- una regla explicita de negocio, por ejemplo `SinExpiracion`.

### Regla 5. Comparaciones temporales

Las comparaciones deben hacerse siempre contra `clock.UtcNow`.

No se debe comparar contra:

- `DateTime.Now`
- `DateTimeOffset.Now`
- `DateTime.UtcNow` disperso por el codigo

### Regla 6. Serializacion

Toda API que exponga un instante debe emitirlo en ISO 8601 con offset UTC.

Ejemplo correcto:

```json
"createdAtUtc": "2026-04-07T21:15:30+00:00"
```

No se deben exponer timestamps sin zona o con hora local implícita.

### Regla 7. Entrada de datos

Si una API recibe un timestamp:

- se acepta `DateTimeOffset`;
- se normaliza a UTC antes de persistir;
- si el cliente envia offset distinto de `+00:00`, se convierte a UTC;
- si la regla de negocio exige UTC estricto, se rechaza con validacion explicita.

### Regla 8. Nombres

Los nuevos campos de instante deben terminar idealmente en `Utc`.

Ejemplos:

- `CreatedAtUtc`
- `ExpiresAtUtc`
- `ConfirmedAtUtc`

Si existe un nombre historico como `FechaCreacion`, debe documentarse que su valor es UTC.

## Reglas Obligatorias de Persistencia

### PostgreSQL

Los instantes UTC deben persistirse como `timestamp with time zone`.

### SQL Server

Los instantes UTC deben persistirse como `datetimeoffset`.

### DateOnly

En ambos motores debe persistirse como `date`.

### Configuracion EF recomendada

Para instantes:

```csharp
builder.Property(x => x.CreatedAtUtc)
    .HasColumnType(databaseProvider == "postgresql"
        ? "timestamp with time zone"
        : "datetimeoffset");
```

Para fechas de calendario:

```csharp
builder.Property(x => x.FechaDeNacimiento)
    .HasColumnType("date");
```

## Regla Transitoria para Npgsql

Mientras exista codigo legado mezclando `Now`, `UtcNow`, `DateTime`, `DateTimeOffset` o sentinelas, se puede mantener temporalmente:

```csharp
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
```

Esta bandera debe considerarse temporal. El objetivo final es eliminarla una vez que:

- todos los instantes persistidos sean UTC;
- no existan comparaciones con hora local;
- las migraciones y seeds sean consistentes;
- la aplicacion pase smoke tests en PostgreSQL y SQL Server.

## Estandar de Migracion de Campos Existentes

### Conversion obligatoria

- `DateTime` persistido que representa instante: convertir a `DateTimeOffset`.
- `DateTime?` persistido que representa instante: convertir a `DateTimeOffset?`.
- `DateTime` de calendario: convertir a `DateOnly`.

### Reglas de nulabilidad

- fecha opcional: tipo nullable;
- fecha requerida: tipo no nullable;
- vigencia indefinida: no usar `MaxValue`, usar `null` o un estado de negocio.

## Antes y Despues del Repositorio

### Caso 1. Timestamp de respuesta

Archivo actual: [CommandResponse.cs](C:/necro/OneDrive/proyectos/DST/dev/AUT2Services/src/AUT2Services.Domain.Core/Commands/CommandResponse.cs)

Antes:

```csharp
public CommandResponse()
{
    Timestamp = DateTimeOffset.Now;
    ValidationResult = new ValidationResult();
    Result = false;
    Data = string.Empty;
}
```

Despues:

```csharp
public CommandResponse(IClock clock)
{
    TimestampUtc = clock.UtcNow;
    ValidationResult = new ValidationResult();
    Result = false;
    Data = string.Empty;
}

public DateTimeOffset TimestampUtc { get; private set; }
```

### Caso 2. Limpieza de refresh tokens

Archivo actual: [ExpiredRefreshTokenCleanupService.cs](C:/necro/OneDrive/proyectos/DST/dev/AUT2Services/src/AUT2Services.Infra.Security/Services/ExpiredRefreshTokenCleanupService.cs)

Antes:

```csharp
var nowUtc = DateTime.UtcNow;

var expiredTokens = await dbContext.RefreshTokens
    .Where(x => x.ExpiresAtUtc <= nowUtc)
    .ToListAsync(cancellationToken);
```

Despues:

```csharp
var nowUtc = clock.UtcNow;

var expiredTokens = await dbContext.RefreshTokens
    .Where(x => x.ExpiresAtUtc <= nowUtc)
    .ToListAsync(cancellationToken);
```

Y el modelo:

```csharp
public DateTimeOffset ExpiresAtUtc { get; set; }
```

### Caso 3. Vigencia de politicas

Archivo actual: [SecurityRepository.cs](C:/necro/OneDrive/proyectos/DST/dev/AUT2Services/src/AUT2Services.Infra.Security/Services/SecurityRepository.cs)

Antes:

```csharp
&& (politicaAsignada.FechaInicioAsignacion <= DateTimeOffset.Now
    && politicaAsignada.FechaTerminoAsignacion >= DateTimeOffset.Now)
```

Despues:

```csharp
var nowUtc = clock.UtcNow;

&& politicaAsignada.FechaInicioAsignacionUtc <= nowUtc
&& (politicaAsignada.FechaTerminoAsignacionUtc == null
    || politicaAsignada.FechaTerminoAsignacionUtc >= nowUtc)
```

### Caso 4. Seed de datos

Archivo actual: [IncludeBaseData.cs](C:/necro/OneDrive/proyectos/DST/dev/AUT2Services/src/AUT2Services.Infra.Data/Extensions/IncludeBaseData.cs)

Antes:

```csharp
fechaInicioAutorizacion: DateTimeOffset.MinValue,
fechaTerminoAutorizacion: DateTimeOffset.MaxValue,
fechaCreacion: DateTimeOffset.Now,
```

Despues:

```csharp
fechaInicioAutorizacionUtc: SeedClock.FixedUtc("2026-04-07T00:00:00+00:00"),
fechaTerminoAutorizacionUtc: null,
fechaCreacionUtc: SeedClock.FixedUtc("2026-04-07T00:00:00+00:00"),
```

Notas:

- los seeds no deben depender de la hora local del equipo;
- para seeds conviene usar valores fijos y deterministas;
- `null` representa vigencia abierta cuando la regla de negocio lo permita.

### Caso 5. Fecha de nacimiento

Archivos actuales:

- [RegisterViewModel.cs](C:/necro/OneDrive/proyectos/DST/dev/AUT2Services/src/AUT2Services.Infra.Security/ViewModels/RegisterViewModel.cs)
- [RegisterCommandViewModel.cs](C:/necro/OneDrive/proyectos/DST/dev/AUT2Services/src/AUT2Services.Infra.Security/ViewModels/RegisterCommandViewModel.cs)
- [InformacionAdicionalModel.cs](C:/necro/OneDrive/proyectos/DST/dev/AUT2Services/src/AUT2Services.Infra.Security/Models/InformacionAdicionalModel.cs)

Antes:

```csharp
public DateTime? FechaDeNacimiento { get; set; } = DateTime.MinValue;
```

Despues:

```csharp
public DateOnly? FechaDeNacimiento { get; set; }
```

## Reglas de Validacion

### Validaciones obligatorias

- un timestamp requerido no puede ser `null`;
- un timestamp persistido debe quedar en UTC;
- una fecha de calendario no debe pasar por conversion a zona horaria;
- una fecha final no puede ser menor que la inicial;
- no se aceptan sentinelas como valores validos de entrada.

### Validacion recomendada para UTC estricto

```csharp
RuleFor(x => x.ExpiresAtUtc)
    .Must(x => x.Offset == TimeSpan.Zero)
    .WithMessage("El timestamp debe estar en UTC.");
```

## Reglas para Consultas y Dominio

- el dominio no debe depender de zona local del servidor;
- el dominio no debe construir fechas con `Now`;
- si una vigencia no tiene fin, debe modelarse como `null` o como estado de negocio;
- si se necesita presentar una fecha en zona local, la conversion ocurre fuera del dominio y fuera de la persistencia.

## Regla para Integraciones Externas

Cuando una integracion externa envie hora local o un offset no UTC:

1. se recibe como `DateTimeOffset`;
2. se transforma a UTC en la capa de entrada;
3. se persiste como UTC;
4. se registra la conversion solo si es relevante para auditoria.

## Checklist de Cumplimiento

Una tarea sobre fechas se considera cerrada solo si cumple:

- no introduce `Now`;
- no introduce `DateTime` para instantes persistidos;
- no introduce sentinelas;
- usa `IClock`;
- persiste instantes en UTC;
- pasa smoke tests en PostgreSQL;
- pasa smoke tests en SQL Server;
- no requiere `LegacyTimestampBehavior` para codigo nuevo.

## Plan de Adopcion para AUT2Services

### Fase 1. Regla de codigo nuevo

Desde este documento, todo codigo nuevo debe cumplir el estandar aunque el legado aun no este convertido.

### Fase 2. Campos de seguridad

Primero se migran:

- refresh tokens;
- expiraciones;
- sesiones;
- cookies;
- claims de expiracion.

### Fase 3. Campos de vigencia de negocio

Luego:

- `FechaInicioAutorizacion`;
- `FechaTerminoAutorizacion`;
- `FechaInicioAsignacion`;
- `FechaTerminoAsignacion`;
- `FechaCreacion`.

### Fase 4. Fechas de calendario

Finalmente:

- `FechaDeNacimiento` a `DateOnly`.

### Fase 5. Retiro de compatibilidad legacy

Al final:

- quitar `Npgsql.EnableLegacyTimestampBehavior`;
- regenerar migraciones si aplica;
- validar ambos motores.

## Estado Deseado

El estado objetivo del proyecto es:

- instantes en `DateTimeOffset` UTC;
- fechas humanas en `DateOnly`;
- sin hora local;
- sin sentinelas;
- sin switch legacy de Npgsql;
- con el mismo comportamiento en PostgreSQL y SQL Server.
