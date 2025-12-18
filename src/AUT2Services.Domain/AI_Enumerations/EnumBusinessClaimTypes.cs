using AUT2Services.Domain.Core.Enumerations;

namespace AUT2Services.Domain.AI_Enumerations;

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
    public const string ACCESS_TOKEN_TYPE = nameof(ACCESS_TOKEN_TYPE);

    public static IList<string> ObtenerListaValores()
    {
        return EnumUtils.GetAllPublicConstantValues<string>(typeof(EnumBusinessClaimTypes));
    }
}
