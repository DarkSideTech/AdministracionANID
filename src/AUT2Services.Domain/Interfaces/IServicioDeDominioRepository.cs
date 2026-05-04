using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Interfaces;

public interface IServicioDeDominioRepository
{

    Task<IEnumerable<UnidadOrganizacionalDTO>> BuscarUnidadesOrganizacionalesPor_Id_Usuario_Id_Organizacion(Guid id_Usuario, Guid id_Organizacion);

    Task<IEnumerable<EntidadParaAsignarPoliticaDTO>> BuscarEntidadesParaAsignarPolitica(Guid id_Usuario, Guid id_UnidadOrganizacional, DateTimeOffset fechaConsulta);

    Task<IEnumerable<EntidadParaAsignarPoliticaDTO>> BuscarEntidadesParaAsignarPoliticaPor_Id_Organizacion(
        Guid id_Organizacion,
        DateTimeOffset fechaConsulta,
        IReadOnlyCollection<Guid>? ids_UnidadesOrganizacionales);

    Task<IEnumerable<PoliticaAsignadaParaEntidadDTO>> BuscarPoliticasAsignadasPor_Id_Entidad(
        Guid id_Entidad,
        DateTimeOffset fechaConsulta);

    Task<EntidadDTO?> BuscarEntidadPrincipalPor_Id_Usuario_Id_Organizacion(Guid id_Usuario, Guid id_Organizaciona);

    Task<IEnumerable<OrganizacionPorUsuarioDTO>> BuscarOrganizacionesPor_Id_Usuario(Guid id_Usuario);

    Task<bool> UsuarioConRolValidaAsignacionDeRol(Guid id_Usuario, Guid id_Rol_ValidaAsignacionUsuario, Guid id_Organizacion, Guid id_Organizacion_ANID);

    Task<bool> UsuarioConRolValidaEnrrolamiento(Guid id_Usuario, Guid id_Rol_ValidaEnrrolamiento);
}
