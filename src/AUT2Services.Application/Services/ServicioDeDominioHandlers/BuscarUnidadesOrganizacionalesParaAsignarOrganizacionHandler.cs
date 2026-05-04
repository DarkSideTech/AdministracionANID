using AUT2Services.Application.ViewModels.ServiciosDeDominio;
using AUT2Services.Domain.Core.Commands;
using FluentValidation.Results;

namespace AUT2Services.Application.Services.ServicioDeDominioHandlers;

public partial class ServicioDeDominioServiceApp
{
    public async Task<CommandResponse> BuscarUnidadesOrganizacionalesParaAsignarOrganizacion(
        BuscarUnidadesOrganizacionalesParaAsignarOrganizacionServicioDeDominioViewModel command)
    {
        var result = new CommandResponse
        {
            Result = false
        };
        result.ValidationResult.Errors = [];

        if (!UsuarioActualEsAdministradorAnid())
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(
                nameof(BuscarUnidadesOrganizacionalesParaAsignarOrganizacion),
                "El usuario autenticado debe tener rol ADMINISTRADOR para administrar la relacion Organizacion - UnidadOrganizacional."));
            return result;
        }

        if (command.Id_Organizacion is null || command.Id_Organizacion == Guid.Empty)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(
                nameof(BuscarUnidadesOrganizacionalesParaAsignarOrganizacion),
                "El Id_Organizacion es obligatorio."));
            return result;
        }

        var organizacion = await organizacionRepository.BuscarPor_Id(command.Id_Organizacion.Value);
        if (organizacion is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(
                nameof(BuscarUnidadesOrganizacionalesParaAsignarOrganizacion),
                $"La organizacion id [{command.Id_Organizacion}] no existe."));
            return result;
        }

        var items = await servicioDeDominioRepository.BuscarUnidadesOrganizacionalesParaAsignarOrganizacion(
            command.Id_Organizacion.Value);

        result.Result = true;
        result.Data = items.Select(item => new UnidadOrganizacionalParaAsignarOrganizacionViewModel
        {
            IdUnidadOrganizacional = item.IdUnidadOrganizacional,
            IdOrganizacionActual = item.IdOrganizacionActual,
            CodigoUnidadOrganizacional = item.CodigoUnidadOrganizacional,
            NombreUnidadOrganizacional = item.NombreUnidadOrganizacional,
            DescripcionUnidadOrganizacional = item.DescripcionUnidadOrganizacional,
            UnidadOrganizacionalBase = item.UnidadOrganizacionalBase,
            Activo = item.Activo,
            CodigoOrganizacionActual = item.CodigoOrganizacionActual,
            NombreOrganizacionActual = item.NombreOrganizacionActual,
            AsignadaAOrganizacion = item.AsignadaAOrganizacion,
            TieneEntidadPrincipal = item.TieneEntidadPrincipal,
            CantidadEntidadesPrincipales = item.CantidadEntidadesPrincipales
        }).ToArray();

        return result;
    }
}
