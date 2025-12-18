using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace AUT2Services.Infra.Security.Accounts.ValidateEmail;

public class EmailConfirmationTokenCommandHandler : CommandHandler,
    IRequestHandler<EmailConfirmationTokenCommand, CommandResponse>
{
    private readonly UserManager<Usuario> userManager;
    private readonly AUT2ServicesContext aUT2ServicesContext;
    private readonly IMediatorHandler mediator;
    private readonly IEntidadRepository entidadRepository;

    public EmailConfirmationTokenCommandHandler(
        UserManager<Usuario> userManager,
        AUT2ServicesContext aUT2ServicesContext,
        IMediatorHandler mediator,
        IEntidadRepository entidadRepository)
    {
        this.userManager = userManager;
        this.aUT2ServicesContext = aUT2ServicesContext;
        this.mediator = mediator;
        this.entidadRepository = entidadRepository;
    }

    public async Task<CommandResponse> Handle(EmailConfirmationTokenCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;

        if (!command.IsValid())
        {
            return CommandResponse;
        }
        var profile = new ProfileModel();

        try
        {
            var usuario = await userManager.FindByIdAsync(command.UserId);
            if (usuario is null)
            {
                AddError("El usuario no existe, no se puede validar el correo electronico");
                return CommandResponse;
            }
            else
            {
                if(usuario.EmailConfirmed)
                {
                    AddError("El usuario ya cuenta con el correo electronico validado");
                    return CommandResponse;
                }

                using var transaction = await aUT2ServicesContext.Database.BeginTransactionAsync(cancellationToken);
                var result = await this.userManager.ConfirmEmailAsync(usuario, command.ConfirmationToken);
                if (!result.Succeeded)
                {
                    AddError("No se puede validar el correo electronico");
                    await transaction.RollbackAsync(cancellationToken);
                    return CommandResponse;
                }
                
                var numeroDeDocumento = JsonConvert.DeserializeObject<InformacionAdicionalModel>(usuario.InformacionAdicional!)!.NumeroDeDocumento!;
                //var entidad = await entidadRepository.BuscarPor_Codigo(numeroDeDocumento);

                //if (entidad is null)
                //{
                //    try
                //    {
                //        var estructuraAutorizacionCommand = new EstructuraAutorizacionCommand()
                //        {
                //            CorreoElectronico = usuario.Email!,
                //            NumeroDeDocumento = numeroDeDocumento,
                //            Id_Persona = usuario.IdPersona!,
                //            NombreADesplegar = usuario.NombreADesplegar!,
                //            Descripcion = usuario.Descripcion!,
                //            Id_Usuario = Guid.Parse(usuario.Id)
                //        };

                //        var resultCommand = await mediator.SendCommand(estructuraAutorizacionCommand, cancellationToken);

                //        if (!resultCommand.Result)
                //        {
                //            foreach (var item in resultCommand.ValidationResult.Errors)
                //            {
                //                AddError($"{item.ErrorCode} {item.ErrorMessage}");
                //            }
                //            await transaction.RollbackAsync(cancellationToken);
                //        }
                //        else
                //        {
                //            await transaction.CommitAsync(cancellationToken);
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        await transaction.RollbackAsync();
                //        AddError($"Error no manejado al momento de crear un usuario, error: {ex.Message}");
                //    }
                //}
                //else
                //{
                //    await transaction.CommitAsync(cancellationToken);
                //}
            }
        }
        catch (Exception ex)
        {
            AddError($"Error al momento de obtener los datos del usuario, message [{ex.Message}]");
        }

        CommandResponse.Data = string.Empty;
        CommandResponse.Result = true;
        return CommandResponse;
    }
}
