// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.218
// -------------------------------------------------
using AUT2Services.Domain.Core.Enumerations;

namespace AUT2Services.Domain.Enumerations;

public static class EnumComoDesplegarUrlDeProceso
{
    public const string NO_DESPLEGAR = nameof(NO_DESPLEGAR);
    public const string IFRAME = nameof(IFRAME);
    public const string VENTANA = nameof(VENTANA);
    public const string REDIRECCION = nameof(REDIRECCION);

    public static IList<string> ObtenerListaValores()
    {
        return EnumUtils.GetAllPublicConstantValues<string>(typeof(EnumComoDesplegarUrlDeProceso));
    }
}

