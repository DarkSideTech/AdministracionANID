using AUT2Services.Domain.Commands.Entidades.Commands;
using AUT2Services.Domain.Commands.Organizaciones.Commands;
using AUT2Services.Domain.Commands.UnidadesOrganizacionales.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Models;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Infra.Security.Models;
using Newtonsoft.Json;

namespace AUT2Services.Infra.Security.Accounts.BaseEntity;


public class BaseEntityCommandHandler : CommandHandler,
    IRequestHandler<BaseEntityCommand, CommandResponse>
{
    private readonly IMediatorHandler mediator;
    private readonly IOrganizacionRepository organizacionRepository;

    public BaseEntityCommandHandler(
        IMediatorHandler mediator,
        IOrganizacionRepository organizacionRepository
        )
    {
        this.mediator = mediator;
        this.organizacionRepository = organizacionRepository;
    }

    public async Task<CommandResponse> Handle(BaseEntityCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;

        if (!command.IsValid())
        {
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

        var crearUnidadOrganizacionalCommand = new CrearUnidadOrganizacionalCommand(
            Guid.Parse(JsonConvert.DeserializeObject<ResponseSingleId>(resultCrearOrganizacionCommand.Data)!.Id),
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

        var crearEntidadCommand = new CrearEntidadCommand(
            Guid.Parse(JsonConvert.DeserializeObject<ResponseSingleId>(resultCrearUnidadOrganizacionalCommand.Data)!.Id),
            (Guid)command.Id_Usuario!,
            EnumTipoDeEntidad.PERSONA,
            command.CorreoElectronico!
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

        var cambiarEntidadAPrincipalEntidadCommand = new CambiaEntidadAPrincipalEntidadCommand(
            Guid.Parse(JsonConvert.DeserializeObject<ResponseSingleId>(resultCrearEntidadCommand.Data)!.Id)
            );

        var resultCambiarEntidadAPrincipalEntidadCommand = await mediator.SendCommand(crearEntidadCommand, cancellationToken);

        if (!resultCambiarEntidadAPrincipalEntidadCommand.Result)
        {
            foreach (var item in resultCambiarEntidadAPrincipalEntidadCommand.ValidationResult.Errors)
            {
                AddError($"{item.ErrorCode} {item.ErrorMessage}");
            }
            return CommandResponse;
        }

        CommandResponse.Data = JsonConvert.SerializeObject(new BaseEntityRequestModel()
        {
            Id_Entidad = Guid.Parse(JsonConvert.DeserializeObject<ResponseSingleId>(resultCrearEntidadCommand.Data)!.Id),
            Id_UnidadOrganizacional = Guid.Parse(JsonConvert.DeserializeObject<ResponseSingleId>(resultCrearUnidadOrganizacionalCommand.Data)!.Id),
            NombreOrganizacion = command.NombreOrganizacion!
        });
        CommandResponse.Result = true;

        return CommandResponse;
    }
}