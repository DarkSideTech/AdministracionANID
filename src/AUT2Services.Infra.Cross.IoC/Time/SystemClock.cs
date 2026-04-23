using AUT2Services.Domain.Core.Time;

namespace AUT2Services.Infra.Cross.IoC.Time;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
