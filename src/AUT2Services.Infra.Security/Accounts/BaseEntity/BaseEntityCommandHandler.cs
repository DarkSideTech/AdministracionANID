using AUT2Services.Domain.Commands.Entidades.Commands;
using AUT2Services.Domain.Commands.Organizaciones.Commands;
using AUT2Services.Domain.Commands.UnidadesOrganizacionales.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Models;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Infra.Security.Accounts.BaseEntity;


public class BaseEntityCommandHandler : CommandHandler,
    IRequestHandler<BaseEntityCommand, CommandResponse>
{
    private readonly IMediatorHandler mediator;
    private readonly IOrganizacionRepository organizacionRepository;
    private readonly UserManager<Usuario> userManager;

    public BaseEntityCommandHandler(
        IMediatorHandler mediator,
        IOrganizacionRepository organizacionRepository,
        UserManager<Usuario> userManager
        )
    {
        this.mediator = mediator;
        this.organizacionRepository = organizacionRepository;
        this.userManager = userManager;
    }

    public async Task<CommandResponse> Handle(BaseEntityCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;

        if (!command.IsValid())
        {
            return CommandResponse;
        }

        var user = await userManager.Users
                .FirstOrDefaultAsync(x => x.Id == command.Id_Usuario.ToString());

        if (user is null)
        {
            AddError("Usuario no existe, no es posible crear la estructura base");
            return CommandResponse;
        }

        var existeOrganizacion = await organizacionRepository.BuscarPor_Codigo(command.CodigoOrganizacion!);

        if (existeOrganizacion is not null)
        {
            AddError($"Ya existe una Organizacion para el usuario de codigo {command.CodigoOrganizacion}, no es posible crear una nueva");
            return CommandResponse;
        }

        var crearOrganizacionCommand = new CrearOrganizacionCommand(
            string.Empty,
            command.CodigoOrganizacion!,
            command.NombreOrganizacion!,
            command.NombreOrganizacion!
            );

        var resultCrearOrganizacionCommand = await mediator.SendCommand(crearOrganizacionCommand, cancellationToken);

        if (!resultCrearOrganizacionCommand.Result)
        {
            foreach (var item in resultCrearOrganizacionCommand.ValidationResult.Errors)
            {
                AddError($"{item.ErrorCode} {item.ErrorMessage}");
            }
            return CommandResponse;
        }

        var organizacionCreada = resultCrearOrganizacionCommand.GetData<ResponseSingleId>();

        var crearUnidadOrganizacionalCommand = new CrearUnidadOrganizacionalCommand(
            Guid.Parse(organizacionCreada!.Id),
            EnumUnidadOrganizacionalBase.CASA_MATRIZ,
            "Casa Matriz",
            "Unidad Organizacional Principal");

        var resultCrearUnidadOrganizacionalCommand = await mediator.SendCommand(crearUnidadOrganizacionalCommand, cancellationToken);

        if (!resultCrearUnidadOrganizacionalCommand.Result)
        {
            foreach (var item in resultCrearUnidadOrganizacionalCommand.ValidationResult.Errors)
            {
                AddError($"{item.ErrorCode} {item.ErrorMessage}");
            }
            return CommandResponse;
        }

        var unidadCreada = resultCrearUnidadOrganizacionalCommand.GetData<ResponseSingleId>();

        var crearEntidadCommand = new CrearEntidadCommand(
            Guid.Parse(unidadCreada!.Id),
            (Guid)command.Id_Usuario!,
            EnumTipoDeEntidad.PERSONA,
            command.CorreoElectronico ?? string.Empty,
            command.PermitirCorreoElectronicoVacio
            );

        var resultCrearEntidadCommand = await mediator.SendCommand(crearEntidadCommand, cancellationToken);

        if (!resultCrearEntidadCommand.Result)
        {
            foreach (var item in resultCrearEntidadCommand.ValidationResult.Errors)
            {
                AddError($"{item.ErrorCode} {item.ErrorMessage}");
            }
            return CommandResponse;
        }

        var entidadCreada = resultCrearEntidadCommand.GetData<ResponseSingleId>();

        var cambiarEntidadAPrincipalEntidadCommand = new CambiaEntidadAPrincipalEntidadCommand(
            Guid.Parse(entidadCreada!.Id)
            );

        var resultCambiarEntidadAPrincipalEntidadCommand = await mediator.SendCommand(cambiarEntidadAPrincipalEntidadCommand, cancellationToken);

        if (!resultCambiarEntidadAPrincipalEntidadCommand.Result)
        {
            foreach (var item in resultCambiarEntidadAPrincipalEntidadCommand.ValidationResult.Errors)
            {
                AddError($"{item.ErrorCode} {item.ErrorMessage}");
            }
            return CommandResponse;
        }

        CommandResponse.Data = new BaseEntityRequestModel()
        {
            Id_Entidad = Guid.Parse(entidadCreada.Id),
            Id_UnidadOrganizacional = Guid.Parse(unidadCreada.Id),
            NombreOrganizacion = command.NombreOrganizacion!
        };
        CommandResponse.Result = true;

        return CommandResponse;
    }
}
