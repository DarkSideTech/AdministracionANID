// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.216
// -------------------------------------------------
using AUT2Services.Domain.Core.Enumerations;

namespace AUT2Services.Domain.Enumerations;

public static class EnumEstadoDeUsuario
{
    public const string REGISTRADO = nameof(REGISTRADO);
    public const string PROCESO_REGISTRO = nameof(PROCESO_REGISTRO);
    public const string RECHAZADO = nameof(RECHAZADO);

    public static IList<string> ObtenerListaValores()
    {
        return EnumUtils.GetAllPublicConstantValues<string>(typeof(EnumEstadoDeUsuario));
    }
}

