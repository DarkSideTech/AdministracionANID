// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.081
// -------------------------------------------------
using AUT2Services.Domain.Core.Enumerations;

namespace AUT2Services.Domain.Enumerations;

public static class EnumBusinessClaimTypes
{
    public const string ID_USUARIO = nameof(ID_USUARIO);
    public const string NOMBRE_A_DESPLEGAR = nameof(NOMBRE_A_DESPLEGAR);
    public const string CODIGO_ORGANIZACION = nameof(CODIGO_ORGANIZACION);
    public const string NOMBRE_ORGANIZACION = nameof(NOMBRE_ORGANIZACION);
    public const string CODIGO_UNIDAD_ORGANIZACIONAL = nameof(CODIGO_UNIDAD_ORGANIZACIONAL);
    public const string NOMBRE_UNIDAD_ORGANIZACIONAL = nameof(NOMBRE_UNIDAD_ORGANIZACIONAL);
    public const string ID_ENTIDAD = nameof(ID_ENTIDAD);
    public const string PROCESO = nameof(PROCESO);

    public static IList<string> ObtenerListaValores()
    {
        return EnumUtils.GetAllPublicConstantValues<string>(typeof(EnumBusinessClaimTypes));
    }
}

