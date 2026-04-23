namespace AUT2Services.Domain.Core.Time;

public static class ClockContext
{
    private static IClock current = new FallbackClock();

    public static IClock Current
    {
        get => current;
        set => current = value ?? throw new ArgumentNullException(nameof(value));
    }

    private sealed class FallbackClock : IClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}