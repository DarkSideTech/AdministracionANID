// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.081
// -------------------------------------------------
using AUT2Services.Domain.Core.Enumerations;

namespace AUT2Services.Domain.Enumerations;

public static class EnumAuthCookie
{
    public const string ACCESS_TOKEN = nameof(ACCESS_TOKEN);
    public const string REFRESH_TOKEN = nameof(REFRESH_TOKEN);

    public static IList<string> ObtenerListaValores()
    {
        return EnumUtils.GetAllPublicConstantValues<string>(typeof(EnumAuthCookie));
    }
}

