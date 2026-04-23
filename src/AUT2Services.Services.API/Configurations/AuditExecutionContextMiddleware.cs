using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Domain.Core.Time;
using AUT2Services.Domain.Enumerations;
using System.Security.Claims;

namespace AUT2Services.Services.API.Configurations;

public sealed class AuditExecutionContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext httpContext,
        IAuditExecutionContextInitializer auditExecutionContextInitializer,
        IClock clock)
    {
        var correlationId = TryResolveCorrelationId(httpContext.TraceIdentifier);
        var user = httpContext.User;

        auditExecutionContextInitializer.Initialize(
            correlationId,
            clock.UtcNow,
            user.FindFirstValue(EnumBusinessClaimTypes.ID_USUARIO) ?? user.FindFirstValue(ClaimTypes.NameIdentifier),
            user.FindFirstValue(ClaimTypes.Name) ?? user.FindFirstValue(EnumBusinessClaimTypes.NOMBRE_A_DESPLEGAR),
            user.FindFirstValue(ClaimTypes.Email),
            httpContext.Request.Path.Value);

        await next(httpContext);
    }

    private static Guid TryResolveCorrelationId(string? traceIdentifier)
    {
        return Guid.TryParse(traceIdentifier, out var correlationId)
            ? correlationId
            : Guid.NewGuid();
    }
}
