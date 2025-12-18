// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.080
// -------------------------------------------------
using AUT2Services.Domain.Core.Enumerations;

namespace AUT2Services.Domain.Enumerations;

public static class EnumDocumentoDeIdentidad
{
    public const string PASAPORTE = nameof(PASAPORTE);
    public const string RUN = nameof(RUN);
    public const string DNI = nameof(DNI);

    public static IList<string> ObtenerListaValores()
    {
        return EnumUtils.GetAllPublicConstantValues<string>(typeof(EnumDocumentoDeIdentidad));
    }
}

