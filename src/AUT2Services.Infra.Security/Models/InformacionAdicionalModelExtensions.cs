using System.Text.Json;

namespace AUT2Services.Infra.Security.Models;

public static class InformacionAdicionalModelExtensions
{
    public static InformacionAdicionalModel ToInformacionAdicionalModel(this string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return new InformacionAdicionalModel();
        }

        try
        {
            return JsonSerializer.Deserialize<InformacionAdicionalModel>(json) ?? new InformacionAdicionalModel();
        }
        catch
        {
            return new InformacionAdicionalModel();
        }
    }

    public static string ToJson(this InformacionAdicionalModel model)
    {
        return JsonSerializer.Serialize(model);
    }
}
