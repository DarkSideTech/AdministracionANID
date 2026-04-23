using AUT2Services.Domain.Core.Time;

namespace AUT2Services.Tests.Support;

internal sealed class TestClock(DateTimeOffset utcNow) : IClock
{
    public DateTimeOffset UtcNow { get; } = utcNow;
}
