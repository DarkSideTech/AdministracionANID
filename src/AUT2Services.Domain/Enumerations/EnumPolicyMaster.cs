using AUT2Services.Domain.Core.Enumerations;

namespace AUT2Services.Domain.Enumerations;

public static class EnumPolicyMaster
{
    public const string USUARIO_LOGUEADO = nameof(USUARIO_LOGUEADO);
    public const string ADMINISTRADOR = nameof(ADMINISTRADOR);
    public const string ADMINISTRADOR_ENTIDAD = nameof(ADMINISTRADOR_ENTIDAD);
    public const string ADMINISTRADOR_ENTIDAD_UNIDAD = nameof(ADMINISTRADOR_ENTIDAD_UNIDAD);
    public const string ADMINISTRADOR_ENTIDAD_UNIDAD_USUARIO = nameof(ADMINISTRADOR_ENTIDAD_UNIDAD_USUARIO);
    public const string VALIDA_ASIGNACION_ROLES = nameof(VALIDA_ASIGNACION_ROLES);
    public const string VALIDA_ENRROLAMIENTO = nameof(VALIDA_ENRROLAMIENTO);
    public const string RESEND_CONFIRMATION_EMAIL = nameof(RESEND_CONFIRMATION_EMAIL);

    public static IList<string> ObtenerListaValores()
    {
        return EnumUtils.GetAllPublicConstantValues<string>(typeof(EnumPolicyMaster));
    }
}

