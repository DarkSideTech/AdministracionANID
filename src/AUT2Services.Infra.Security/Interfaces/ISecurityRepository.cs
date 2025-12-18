using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Models;

namespace AUT2Services.Infra.Security.Interfaces;

public interface ISecurityRepository
{
    Task<IEnumerable<SecurityClaims>> BuscarTodasLasPolicies(Guid id_Entidad);

    Task<Usuario> BuscarUsuario(string nombreUsuario);

    Task<int> BuscarUltimoIdAutorizacion();

    Task<Entidad> BuscarEntidadPrincipalPorUsuarioOrganizacion(Guid id_Usuario, Guid id_Organizacion);

    Task<Usuario?> BuscarUsuarioPor_RefreshToken(string refreshToken);
}