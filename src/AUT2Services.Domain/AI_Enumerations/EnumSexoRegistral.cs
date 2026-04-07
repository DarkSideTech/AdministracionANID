// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.080
// -------------------------------------------------
using AUT2Services.Domain.Core.Enumerations;

namespace AUT2Services.Domain.Enumerations;

public static class EnumSexoRegistral
{
    public const string FEMENINO = nameof(FEMENINO);
    public const string MASCULINO = nameof(MASCULINO);
    public const string NO_BINARIO = nameof(NO_BINARIO);
    public const string NO_DECLARA = nameof(NO_DECLARA);

    public static IList<string> ObtenerListaValores()
    {
        return EnumUtils.GetAllPublicConstantValues<string>(typeof(EnumSexoRegistral));
    }
}

