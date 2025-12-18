// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.080
// -------------------------------------------------
using AUT2Services.Domain.Core.Enumerations;

namespace AUT2Services.Domain.Enumerations;

public static class EnumRolesBase
{
    public const string ADMINISTRADOR = nameof(ADMINISTRADOR);
    public const string ADMINISTRADOR_ENTIDAD = nameof(ADMINISTRADOR_ENTIDAD);
    public const string ADMINISTRADOR_UNIDAD = nameof(ADMINISTRADOR_UNIDAD);
    public const string VALIDA_ASIGNACION_ROLES = nameof(VALIDA_ASIGNACION_ROLES);
    public const string VALIDA_ENRROLAMIENTO = nameof(VALIDA_ENRROLAMIENTO);
    public const string USUARIO = nameof(USUARIO);

    public static IList<string> ObtenerListaValores()
    {
        return EnumUtils.GetAllPublicConstantValues<string>(typeof(EnumRolesBase));
    }
}

