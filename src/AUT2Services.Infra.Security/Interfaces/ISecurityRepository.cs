using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;

namespace AUT2Services.Infra.Security.Interfaces;

public interface ISecurityRepository
{
    Task<IEnumerable<SecurityClaims>> BuscarTodasLasPolicies(Guid id_Entidad, Guid id_Rol);

    Task<IList<string>> BuscarRolesPor_Id_Entidad(Guid id_Entidad);

    Task<IList<UnidadOrganizacionalEntidadRolPorUsuario>> BuscarUnidadesOrganizacionalesEntidadRolPor_Id_Organizacion(Guid id_Organizacion, Guid id_Usuario);

    Task<Usuario> BuscarUsuario(string nombreUsuario);

    Task<int> BuscarUltimoIdAutorizacion();

    Task<Entidad?> BuscarEntidadPrincipalPorUsuarioOrganizacion(Guid id_Usuario, Guid id_Organizacion);

    Task<Usuario?> BuscarUsuarioPor_RefreshToken(string refreshToken);

    Task<EntidadRolSeleccionado?> BuscarEntidadRolSeleccionadoPor_Id_Entidad(Guid id_Entidad, Guid? id_Rol = null);
}
