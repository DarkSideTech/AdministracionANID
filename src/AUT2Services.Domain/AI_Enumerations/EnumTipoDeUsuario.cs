// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.079
// -------------------------------------------------
using AUT2Services.Domain.Core.Enumerations;

namespace AUT2Services.Domain.Enumerations;

public static class EnumTipoDeUsuario
{
    public const string NACIONAL = nameof(NACIONAL);
    public const string EXTRANJERO = nameof(EXTRANJERO);
    public const string EXTRANJERO_RESIDENTE = nameof(EXTRANJERO_RESIDENTE);
    public const string EXTRANJERO_NACIONALIZADO = nameof(EXTRANJERO_NACIONALIZADO);

    public static IList<string> ObtenerListaValores()
    {
        return EnumUtils.GetAllPublicConstantValues<string>(typeof(EnumTipoDeUsuario));
    }
}

