using System.Text.Json;

namespace AUT2Services.Domain.Core.Models;

public class ResultModel
{
    public bool Result { get; set; }
    public object? Data { get; set; } = string.Empty;
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

