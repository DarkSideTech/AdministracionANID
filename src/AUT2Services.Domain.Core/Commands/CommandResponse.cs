using AUT2Services.Domain.Core.Time;
using FluentValidation.Results;
using System.Text.Json;

namespace AUT2Services.Domain.Core.Commands
{
    public class CommandResponse
    {
        public CommandResponse()
        {
            Timestamp = ClockContext.Current.UtcNow;
            ValidationResult = new ValidationResult();
            Result = false;
            Data = string.Empty;
        }

        public bool Result { get; set; }
        public object? Data { get; set; }
        public DateTimeOffset? Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; }
        public bool HasData => Data switch
        {
            null => false,
            string value => !string.IsNullOrEmpty(value),
            _ => true
        };

        public T? GetData<T>()
        {
            if (Data is null)
            {
                return default;
            }

            if (Data is T value)
            {
                return value;
            }

            if (Data is JsonElement element)
            {
                return element.Deserialize<T>();
            }

            if (Data is string json)
            {
                if (typeof(T) == typeof(string))
                {
                    return (T)(object)json;
                }

                if (string.IsNullOrWhiteSpace(json))
                {
                    return default;
                }

                return JsonSerializer.Deserialize<T>(json);
            }

            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(Data));
        }
    }
}
