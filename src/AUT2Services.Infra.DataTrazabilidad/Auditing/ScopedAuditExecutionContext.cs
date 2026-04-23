using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Domain.Core.Time;

namespace AUT2Services.Infra.DataTrazabilidad.Auditing;

public sealed class ScopedAuditExecutionContext : IAuditExecutionContext, IAuditExecutionContextInitializer
{
    private readonly IClock clock;
    private bool initialized;

    public ScopedAuditExecutionContext(IClock clock)
    {
        this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
        CorrelationId = Guid.NewGuid();
        RequestedAtUtc = this.clock.UtcNow;
    }

    public Guid CorrelationId { get; private set; }
    public string? UserId { get; private set; }
    public string? Username { get; private set; }
    public string? Email { get; private set; }
    public string? RequestPath { get; private set; }
    public DateTimeOffset RequestedAtUtc { get; private set; }

    public void Initialize(
        Guid correlationId,
        DateTimeOffset requestedAtUtc,
        string? userId,
        string? username,
        string? email,
        string? requestPath)
    {
        CorrelationId = correlationId == Guid.Empty ? CorrelationId : correlationId;
        RequestedAtUtc = requestedAtUtc == default ? RequestedAtUtc : requestedAtUtc;
        UserId = userId;
        Username = username;
        Email = email;
        RequestPath = requestPath;
        initialized = true;
    }

    internal void EnsureInitialized()
    {
        if (initialized)
        {
            return;
        }

        CorrelationId = CorrelationId == Guid.Empty ? Guid.NewGuid() : CorrelationId;
        RequestedAtUtc = RequestedAtUtc == default ? clock.UtcNow : RequestedAtUtc;
        initialized = true;
    }
}
