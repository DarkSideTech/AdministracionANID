// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.081
// -------------------------------------------------
using AUT2Services.Domain.Core.Enumerations;

namespace AUT2Services.Domain.Enumerations;

public static class EnumPartialBusinessClaimTypes
{
    public const string _ID_PROCESO = nameof(_ID_PROCESO);
    public const string _ID_MACRO_PROCESO = nameof(_ID_MACRO_PROCESO);
    public const string _NOMBRE = nameof(_NOMBRE);
    public const string _NIVEL_DE_PROCESO = nameof(_NIVEL_DE_PROCESO);
    public const string _ROL = nameof(_ROL);
    public const string _TOKEN = nameof(_TOKEN);
    public const string _URL = nameof(_URL);
    public const string _COMO_DESPLEGAR_URL = nameof(_COMO_DESPLEGAR_URL);

    public static IList<string> ObtenerListaValores()
    {
        return EnumUtils.GetAllPublicConstantValues<string>(typeof(EnumPartialBusinessClaimTypes));
    }
}

