namespace AUT2Services.Domain.Core.Time;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
