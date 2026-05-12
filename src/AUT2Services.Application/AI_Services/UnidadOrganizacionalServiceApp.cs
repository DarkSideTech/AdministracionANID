// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 14:21:10.427
// -------------------------------------------------
using AUT2Services.Application.Extensions;
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.UnidadesOrganizacionales;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Infra.Security.Interfaces;
using FluentValidation.Results;

namespace AUT2Services.Application.Services;

public class UnidadOrganizacionalServiceApp : IUnidadOrganizacionalServiceApp
{
        private readonly IUnidadOrganizacionalRepository _unidadOrganizacionalRepository;
        private readonly IServicioDeDominioRepository _servicioDeDominioRepository;
        private readonly IUserAccessor _userAccessor;
        private readonly IMediatorHandler _mediator;

    public UnidadOrganizacionalServiceApp(
                IUnidadOrganizacionalRepository unidadOrganizacionalRepository,
                IServicioDeDominioRepository servicioDeDominioRepository,
                IUserAccessor userAccessor,
                IMediatorHandler mediator
        )
    {
                _unidadOrganizacionalRepository = unidadOrganizacionalRepository ?? throw new ArgumentNullException(nameof(unidadOrganizacionalRepository));
                _servicioDeDominioRepository = servicioDeDominioRepository ?? throw new ArgumentNullException(nameof(servicioDeDominioRepository));
                _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<CommandResponse> Crear(CrearUnidadOrganizacionalViewModel command)
    {
        var validationResponse = await ValidarPuedeOperarOrganizacion(command.Id_Organizacion, nameof(Crear));
        if (validationResponse is not null)
        {
            return validationResponse;
        }

        return await _mediator.SendCommand(command.ToCrearCommand());
    }

    public async Task<CommandResponse> Modificar(ModificarUnidadOrganizacionalViewModel command)
    {
        var (entityResponse, idOrganizacion) = await ObtenerIdOrganizacionDesdeUnidad(command.Id, nameof(Modificar));
        if (entityResponse is not null)
        {
            return entityResponse;
        }

        var validationResponse = await ValidarPuedeOperarOrganizacion(idOrganizacion, nameof(Modificar));
        if (validationResponse is not null)
        {
            return validationResponse;
        }

        return await _mediator.SendCommand(command.ToModificarCommand());
    }

    public async Task<CommandResponse> Eliminar(EliminarUnidadOrganizacionalViewModel command)
    {
        var (entityResponse, idOrganizacion) = await ObtenerIdOrganizacionDesdeUnidad(command.Id, nameof(Eliminar));
        if (entityResponse is not null)
        {
            return entityResponse;
        }

        var validationResponse = await ValidarPuedeOperarOrganizacion(idOrganizacion, nameof(Eliminar));
        if (validationResponse is not null)
        {
            return validationResponse;
        }

        return await _mediator.SendCommand(command.ToEliminarCommand());
    }

    public async Task<CommandResponse> EliminarPor_Codigo_Id_Organizacion(EliminarPor_Codigo_Id_OrganizacionUnidadOrganizacionalViewModel command)
    {
        var validationResponse = await ValidarPuedeOperarOrganizacion(command.Id_Organizacion, nameof(EliminarPor_Codigo_Id_Organizacion));
        if (validationResponse is not null)
        {
            return validationResponse;
        }

        return await _mediator.SendCommand(command.ToEliminarPor_Codigo_Id_OrganizacionCommand());
    }

    public async Task<CommandResponse> Activar(ActivarUnidadOrganizacionalViewModel command)
    {
        var (entityResponse, idOrganizacion) = await ObtenerIdOrganizacionDesdeUnidad(command.Id, nameof(Activar));
        if (entityResponse is not null)
        {
            return entityResponse;
        }

        var validationResponse = await ValidarPuedeOperarOrganizacion(idOrganizacion, nameof(Activar));
        if (validationResponse is not null)
        {
            return validationResponse;
        }

        return await _mediator.SendCommand(command.ToActivarCommand());
    }

    public async Task<CommandResponse> Desactivar(DesactivarUnidadOrganizacionalViewModel command)
    {
        var (entityResponse, idOrganizacion) = await ObtenerIdOrganizacionDesdeUnidad(command.Id, nameof(Desactivar));
        if (entityResponse is not null)
        {
            return entityResponse;
        }

        var validationResponse = await ValidarPuedeOperarOrganizacion(idOrganizacion, nameof(Desactivar));
        if (validationResponse is not null)
        {
            return validationResponse;
        }

        return await _mediator.SendCommand(command.ToDesactivarCommand());
    }

    public async Task<IEnumerable<UnidadOrganizacionalViewModel>> BuscarTodos( 
        )
    {
        return (await _unidadOrganizacionalRepository.BuscarTodos( 
        )).ToViewModel(); 
    } 

    public async Task<UnidadOrganizacionalViewModel> BuscarPor_Id( 
        Guid id 
        )
    {
        return (await _unidadOrganizacionalRepository.BuscarPor_Id( 
            id 
        )).ToViewModel(); 
    } 

    public async Task<UnidadOrganizacionalViewModel> BuscarPor_Codigo_Id_Organizacion( 
        string codigo, 
        Guid id_Organizacion 
        )
    {
        return (await _unidadOrganizacionalRepository.BuscarPor_Codigo_Id_Organizacion( 
            codigo, 
            id_Organizacion 
        )).ToViewModel(); 
    } 

    public async Task<IEnumerable<UnidadOrganizacionalViewModel>> BuscarPor_Id_Organizacion( 
        Guid id_Organizacion 
        )
    {
        return (await _unidadOrganizacionalRepository.BuscarPor_Id_Organizacion( 
            id_Organizacion 
        )).ToViewModel(); 
    } 

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    private async Task<(CommandResponse? ErrorResponse, Guid IdOrganizacion)> ObtenerIdOrganizacionDesdeUnidad(Guid? idUnidadOrganizacional, string operationName)
    {
        if (idUnidadOrganizacional is null || idUnidadOrganizacional == Guid.Empty)
        {
            return (CrearErrorResponse(operationName, "El Id de unidad organizacional es obligatorio para validar la autorizacion contextual."), Guid.Empty);
        }

        var unidadOrganizacional = await _unidadOrganizacionalRepository.BuscarPor_Id(idUnidadOrganizacional.Value);
        if (unidadOrganizacional is null)
        {
            return (CrearErrorResponse(operationName, $"La unidad organizacional [{idUnidadOrganizacional}] no existe."), Guid.Empty);
        }

        return (null, unidadOrganizacional.Id_Organizacion);
    }

    private async Task<CommandResponse?> ValidarPuedeOperarOrganizacion(Guid? idOrganizacion, string operationName)
    {
        if (idOrganizacion is null || idOrganizacion == Guid.Empty)
        {
            return CrearErrorResponse(operationName, "El Id_Organizacion es obligatorio para validar la autorizacion contextual.");
        }

        if (UsuarioActualTieneRol(EnumRolesBase.ADMINISTRADOR))
        {
            return null;
        }

        if (!UsuarioActualTieneRol(EnumRolesBase.ADMINISTRADOR_ENTIDAD))
        {
            return CrearErrorResponse(operationName, "El usuario autenticado no tiene el rol ADMINISTRADOR_ENTIDAD para operar unidades organizacionales.");
        }

        if (!Guid.TryParse(_userAccessor.GetIdUsuario(), out var idUsuario))
        {
            return CrearErrorResponse(operationName, "No fue posible resolver el usuario autenticado para validar la autorizacion contextual.");
        }

        var autorizado = await _servicioDeDominioRepository.UsuarioConRolEnOrganizacion(
            idUsuario,
            idOrganizacion.Value,
            EnumRolesBase.ADMINISTRADOR_ENTIDAD,
            DateTimeOffset.UtcNow);

        return autorizado
            ? null
            : CrearErrorResponse(operationName, $"El usuario autenticado no administra la organizacion [{idOrganizacion}] con el rol ADMINISTRADOR_ENTIDAD.");
    }

    private bool UsuarioActualTieneRol(string role)
    {
        var roles = (_userAccessor.GetRoles() ?? new List<string>())
            .Concat(_userAccessor.GetRolesPorProceso(EnumProcesosBase.ADMINISTRACION) ?? new List<string>());

        return roles.Any(currentRole => string.Equals(currentRole, role, StringComparison.OrdinalIgnoreCase));
    }

    private static CommandResponse CrearErrorResponse(string propertyName, string message)
    {
        var response = new CommandResponse
        {
            Result = false
        };
        response.ValidationResult.Errors.Add(new ValidationFailure(propertyName, message));
        return response;
    }
}

