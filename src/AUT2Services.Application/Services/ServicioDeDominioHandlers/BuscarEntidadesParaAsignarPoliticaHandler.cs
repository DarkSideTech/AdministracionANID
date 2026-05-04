using AUT2Services.Application.ViewModels.ServiciosDeDominio;
using FluentValidation.Results;

namespace AUT2Services.Application.Services.ServicioDeDominioHandlers;

public partial class ServicioDeDominioServiceApp
{
    public async Task<IEnumerable<EntidadParaAsignarPoliticaViewModel>> BuscarEntidadesParaAsignarPolitica(
        Guid id_Usuario,
        Guid id_UnidadOrganizacional)
    {
        if (id_Usuario == Guid.Empty || id_UnidadOrganizacional == Guid.Empty)
        {
            return [];
        }

        var unidadOrganizacional = await unidadOrganizacionalRepository.BuscarPor_Id(id_UnidadOrganizacional);
        if (unidadOrganizacional is null)
        {
            return [];
        }

        var validationErrors = new List<ValidationFailure>();
        var puedeOperarUnidad = await UsuarioActualPuedeOperarUnidadOrganizacional(
            unidadOrganizacional,
            nameof(BuscarEntidadesParaAsignarPolitica),
            validationErrors);

        if (!puedeOperarUnidad)
        {
            return [];
        }

        var resultGet = await servicioDeDominioRepository.BuscarEntidadesParaAsignarPolitica(
            id_Usuario,
            id_UnidadOrganizacional,
            clock.UtcNow);

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
