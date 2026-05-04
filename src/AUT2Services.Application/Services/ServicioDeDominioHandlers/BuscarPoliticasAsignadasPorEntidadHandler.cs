using AUT2Services.Application.ViewModels.ServiciosDeDominio;
using FluentValidation.Results;

namespace AUT2Services.Application.Services.ServicioDeDominioHandlers;

public partial class ServicioDeDominioServiceApp
{
    public async Task<IEnumerable<PoliticaAsignadaParaEntidadViewModel>> BuscarPoliticasAsignadasPorEntidad(
        Guid id_Entidad)
    {
        if (id_Entidad == Guid.Empty || !UsuarioActualTieneRolAdministradorContextual())
        {
            return [];
        }

        var entidad = await entidadRepository.BuscarPor_Id(id_Entidad);
        if (entidad is null)
        {
            return [];
        }

        var unidadOrganizacional = await unidadOrganizacionalRepository.BuscarPor_Id(entidad.Id_UnidadOrganizacional);
        if (unidadOrganizacional is null)
        {
            return [];
        }

        var validationErrors = new List<ValidationFailure>();
        var puedeOperarUnidad = await UsuarioActualPuedeOperarUnidadOrganizacional(
            unidadOrganizacional,
            nameof(BuscarPoliticasAsignadasPorEntidad),
            validationErrors);

        if (!puedeOperarUnidad)
        {
            return [];
        }

        var resultGet = await servicioDeDominioRepository.BuscarPoliticasAsignadasPor_Id_Entidad(
            id_Entidad,
            clock.UtcNow);

        return resultGet.Select(item => new PoliticaAsignadaParaEntidadViewModel
        {
            IdPoliticaAsignada = item.IdPoliticaAsignada,
            IdEntidad = item.IdEntidad,
            IdRol = item.IdRol,
            NombreRol = item.NombreRol,
            IdProceso = item.IdProceso,
            CodigoProceso = item.CodigoProceso,
            NombreProceso = item.NombreProceso,
            RolRequiereValidacion = item.RolRequiereValidacion,
            RolAsignadoValidado = item.RolAsignadoValidado
        });
    }
}
