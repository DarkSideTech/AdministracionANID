using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Enumerations;
using FluentValidation.Results;

namespace AUT2Services.Application.Services.ServicioDeDominioHandlers;

public partial class ServicioDeDominioServiceApp
{
    private async Task<bool> UsuarioActualPuedeOperarUnidadOrganizacional(
        UnidadOrganizacional unidadOrganizacional,
        string operationName,
        IList<ValidationFailure> errors)
    {
        if (UsuarioActualEsAdministradorAnid())
        {
            return true;
        }

        if (!Guid.TryParse(userAccessor.GetIdUsuario(), out var idUsuarioActual))
        {
            errors.Add(new ValidationFailure(operationName, "No fue posible resolver el usuario autenticado para validar la autorización contextual."));
            return false;
        }

        var unidadesAutorizadas = await servicioDeDominioRepository.BuscarUnidadesOrganizacionalesPor_Id_Usuario_Id_Organizacion(
            idUsuarioActual,
            unidadOrganizacional.Id_Organizacion);

        var puedeOperar = unidadesAutorizadas.Any(item => item.Id == unidadOrganizacional.Id);
        if (puedeOperar)
        {
            return true;
        }

        errors.Add(new ValidationFailure(
            operationName,
            $"El usuario autenticado no tiene autorización para operar sobre la unidad organizacional [{unidadOrganizacional.Id}]."));
        return false;
    }

    private bool UsuarioActualEsAdministradorAnid()
    {
        var roles = userAccessor.GetRoles()
            .Concat(userAccessor.GetRolesPorProceso(EnumProcesosBase.ADMINISTRACION));

        return roles.Any(role => string.Equals(role, EnumRolesBase.ADMINISTRADOR, StringComparison.OrdinalIgnoreCase));
    }

    private bool UsuarioActualTieneRolAdministradorContextual()
    {
        var roles = userAccessor.GetRoles()
            .Concat(userAccessor.GetRolesPorProceso(EnumProcesosBase.ADMINISTRACION));

        return roles.Any(role =>
            string.Equals(role, EnumRolesBase.ADMINISTRADOR, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(role, EnumRolesBase.ADMINISTRADOR_ENTIDAD, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(role, EnumRolesBase.ADMINISTRADOR_UNIDAD, StringComparison.OrdinalIgnoreCase));
    }
}
