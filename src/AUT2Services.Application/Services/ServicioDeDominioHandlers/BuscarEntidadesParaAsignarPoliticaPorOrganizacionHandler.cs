using AUT2Services.Application.ViewModels.ServiciosDeDominio;

namespace AUT2Services.Application.Services.ServicioDeDominioHandlers;

public partial class ServicioDeDominioServiceApp
{
    public async Task<IEnumerable<EntidadParaAsignarPoliticaViewModel>> BuscarEntidadesParaAsignarPoliticaPorOrganizacion(
        Guid id_Organizacion)
    {
        if (id_Organizacion == Guid.Empty || !UsuarioActualTieneRolAdministradorContextual())
        {
            return [];
        }

        IReadOnlyCollection<Guid>? idsUnidadesAutorizadas = null;
        if (!UsuarioActualEsAdministradorAnid())
        {
            if (!Guid.TryParse(userAccessor.GetIdUsuario(), out var idUsuarioActual))
            {
                return [];
            }

            idsUnidadesAutorizadas = (await servicioDeDominioRepository.BuscarUnidadesOrganizacionalesPor_Id_Usuario_Id_Organizacion(
                    idUsuarioActual,
                    id_Organizacion))
                .Select(item => item.Id)
                .Distinct()
                .ToArray();
        }

        var resultGet = await servicioDeDominioRepository.BuscarEntidadesParaAsignarPoliticaPor_Id_Organizacion(
            id_Organizacion,
            clock.UtcNow,
            idsUnidadesAutorizadas);

        return resultGet.Select(item => new EntidadParaAsignarPoliticaViewModel
        {
            IdEntidad = item.IdEntidad,
            IdUsuario = item.IdUsuario,
            NombreUsuario = item.NombreUsuario,
            IdUnidadOrganizacional = item.IdUnidadOrganizacional,
            CodigoUnidadOrganizacional = item.CodigoUnidadOrganizacional,
            NombreUnidadOrganizacional = item.NombreUnidadOrganizacional,
            TipoDeEntidad = item.TipoDeEntidad,
            CorreoElectronico = item.CorreoElectronico,
            Principal = item.Principal
        });
    }
}
