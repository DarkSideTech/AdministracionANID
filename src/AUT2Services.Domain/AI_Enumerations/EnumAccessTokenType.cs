// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.081
// -------------------------------------------------
using AUT2Services.Domain.Core.Enumerations;

namespace AUT2Services.Domain.Enumerations;

public static class EnumAccessTokenType
{
    public const string LOGIN = nameof(LOGIN);
    public const string LOGIN_ORGANIZATION = nameof(LOGIN_ORGANIZATION);

    public static IList<string> ObtenerListaValores()
    {
        return EnumUtils.GetAllPublicConstantValues<string>(typeof(EnumAccessTokenType));
    }
}

