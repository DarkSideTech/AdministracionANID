using AUT2Services.Domain.Core.Enumerations;

namespace AUT2Services.Domain.AI_Enumerations;

public static class EnumAccessTokenType
{
    public const string LOGIN = nameof(LOGIN);
    public const string LOGIN_ORGANIZATION = nameof(LOGIN_ORGANIZATION);

    public static IList<string> ObtenerListaValores()
    {
        return EnumUtils.GetAllPublicConstantValues<string>(typeof(EnumBusinessClaimTypes));
    }
}
