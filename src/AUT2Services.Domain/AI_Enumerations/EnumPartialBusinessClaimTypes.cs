using AUT2Services.Domain.Core.Enumerations;

namespace AUT2Services.Domain.AI_Enumerations;

public static class EnumPartialBusinessClaimTypes
{
    public const string _NOMBRE = nameof(_NOMBRE);
    public const string _TOKEN = nameof(_TOKEN);
    public const string _URL = nameof(_URL);
    public const string _COMO_DESPLEGAR_URL = nameof(_COMO_DESPLEGAR_URL);

    public static IList<string> ObtenerListaValores()
    {
        return EnumUtils.GetAllPublicConstantValues<string>(typeof(EnumPartialBusinessClaimTypes));
    }
}
