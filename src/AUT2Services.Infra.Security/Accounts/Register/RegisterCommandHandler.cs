using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Enumerations;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Domain.Core.Models;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Accounts.BaseEntity;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace AUT2Services.Infra.Security.Accounts.Register;
public class RegisterCommandHandler : CommandHandler,
    IRequestHandler<RegisterCommand, CommandResponse>
{
    private readonly UserManager<Usuario> userManager;
    private readonly IMediatorHandler mediator;
    private readonly AUT2ServicesContext aUT2ServicesContext;
    private readonly IConfiguration configuration;
    private readonly IEntidadRepository entidadRepository;
    private readonly IEmailMessageSender emailSender;
    private readonly SendEmailOptions sendEmailOptions;

    public RegisterCommandHandler(
        UserManager<Usuario> userManager,
        IMediatorHandler mediator,
        AUT2ServicesContext aUT2ServicesContext,
        IConfiguration configuration,
        IEntidadRepository entidadRepository,
        IOptions<SendEmailOptions> sendEmailOptions,
        IEmailMessageSender emailSender)
    {
        this.userManager = userManager;
        this.configuration = configuration;
        this.aUT2ServicesContext = aUT2ServicesContext;
        this.mediator = mediator;
        this.entidadRepository = entidadRepository;
        this.emailSender = emailSender;
        this.sendEmailOptions = sendEmailOptions.Value;
    }

    public async Task<CommandResponse> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;

        if (!command.IsValid())
        {
            return CommandResponse;
        }

        if (await userManager.Users.AnyAsync(x => x.Email == command.CorreoElectronico, cancellationToken: cancellationToken))
        {
            AddError("El Correo Electronico ya fue registrado por otro usuario");
            return CommandResponse;
        }

        if (await userManager.Users.AnyAsync(x => x.UserName == command.CorreoElectronico, cancellationToken: cancellationToken))
        {
            AddError("El Nombre De Usuario ya fue registrado por otro usuario");
            return CommandResponse;
        }

        using var transaction = await aUT2ServicesContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var userName = string.IsNullOrEmpty(command.NombreUsuario) ? command.CorreoElectronico : command.NombreUsuario;
            var nombreADesplegar = $"{command.PrimerNombre!.Trim()} {command.PrimerApellido!.Trim()}";

            var nuevoUsuario = new Usuario
            {
                UserName = userName,
                NormalizedUserName = userName!.ToUpper(),
                Email = command.CorreoElectronico,
                NormalizedEmail = command.CorreoElectronico!.ToUpper(),
                EmailConfirmed = command.TipoDeUsuario!.Equals(EnumTipoDeUsuario.NACIONAL),
                PhoneNumber = command.NumeroDeTelefono ?? string.Empty,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                AccessFailedCount = int.Parse(configuration["MaximaCantidadIntentosFallidos"] ?? "5"),
                IdPersona = command.IdPersona,
                NombreADesplegar = nombreADesplegar,
                Descripcion = command.Descripcion,
                TipoDeUsuario = command.TipoDeUsuario,
                Activo = true,
                UsuarioBase = false,
                RequiereValidacionEnrrolamiento = !command.TipoDeUsuario!.Equals(EnumTipoDeUsuario.NACIONAL),
                EstadoDeUsuario = command.TipoDeUsuario!.Equals(EnumTipoDeUsuario.NACIONAL) ?
                    EnumEstadoDeUsuario.REGISTRADO
                    : EnumEstadoDeUsuario.PROCESO_REGISTRO,
                InformacionAdicional = JsonConvert.SerializeObject(new InformacionAdicionalModel()
                    {
                        Nacionalidad = command.Nacionalidad,
                        DocumentoDeIdentidad = command.DocumentoDeIdentidad,
                        NumeroDeDocumento = command.NumeroDeDocumento,
                        CodigoValidadorDocumento = command.CodigoValidadorDocumento,
                        PrimerNombre = command.PrimerNombre,
                        SegundoNombre = command.SegundoNombre,
                        PrimerApellido = command.PrimerApellido,
                        SegundoApellido = command.SegundoApellido,
                        SexoDeclarativo = command.SexoDeclarativo,
                        SexoRegistral = command.SexoRegistral,
                        FechaDeNacimiento = command.FechaDeNacimiento,
                        TerminosYCondiciones = command.TerminosYCondiciones
                    }),
            };

            var resultado = await userManager.CreateAsync(nuevoUsuario, command.Contraseña!);

            if (resultado is null || !resultado.Succeeded)
            {
                AddError("No es posible crear el nuevo usuario");

                foreach (var item in resultado!.Errors)
                {
                    AddError($"{item.Code} {item.Description}");
                }

                await transaction.RollbackAsync(cancellationToken);
                return CommandResponse;
            }

            var usuarioExistente = await userManager.FindByEmailAsync(command.CorreoElectronico);
            if (usuarioExistente == null)
            {
                AddError("El usuario buscado no existe, no se puede seguir con el registro del usuario");
                await transaction.RollbackAsync(cancellationToken);
                return CommandResponse;
            }

            var id_Usuario = usuarioExistente.Id;

            if (usuarioExistente.EmailConfirmed.Equals(false) ||
                usuarioExistente.RequiereValidacionEnrrolamiento.Equals(true))
            {
                if (!usuarioExistente.EmailConfirmed)
                {
                    var confirmationEmailToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(await this.userManager.GenerateEmailConfirmationTokenAsync(usuarioExistente)));

                    if (string.IsNullOrEmpty(confirmationEmailToken))
                    {
                        AddError("No es posible generar el token de confirmacion del correo electronico del usuario");
                        await transaction.RollbackAsync(cancellationToken);
                        return CommandResponse;
                    }

                    string emailBody = string.Format($@"
                        <!DOCTYPE html>
                        <html lang=""es"">
	                        <head>
		                        <meta charset=""UTF-8"">
		                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
		                        <title>Correo Automático</title>
	                        </head>
	                        <body style=""margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;"">
		                        <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
			                        <tr>
				                        <td align=""center"" style=""padding: 20px 0;"">
					                        <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1);"">
						                        <tr>
							                        <td align=""center"">
                                                        <h1 style=""color: #333333; margin: 0; font-size: 24px;"">Agencia Nacional de Investigacion y Desarrollo</h1>
							                        </td>
						                        </tr>
						                        <tr>
							                        <td style=""padding: 40px; text-align: center;"">
								                        <h2 style=""color: #333333; margin: 0; font-size: 24px;"">Hola Nuevo Usuario</h2>
								                        <div style=""margin-top: 30px; padding: 20px; background-color: #f8f9fa; border: 2px dashed #007bff; display: inline-block;"">
									                        <span style=""font-size: 18px; font-weight: bold; color: #007bff; letter-spacing: 2px;"">
										                        <a href=""{sendEmailOptions.APIValidateEmail}?email={usuarioExistente.Email}&validationtoken={confirmationEmailToken}"">Link para Validar Cuenta de Correo</a>. 
									                        </span>
								                        </div>
							                        </td>
						                        </tr>
						                        <tr>
							                        <td style=""padding: 10px; background-color: #333333; color: #ffffff; text-align: center; font-size: 12px;"">
								                        <p style=""margin: 0;"">&copy; 2026 Dark Side Tech. Todos los derechos reservados.</p>
							                        </td>
						                        </tr>
					                        </table>
				                        </td>
			                        </tr>
		                        </table>
	                        </body>
                        </html>");

                    var email = new EmailDataModel()
                    {
                        FromMailboxAddresses = [new() { Address = sendEmailOptions.Remitente, Name = "Correo ANID"}],
                        ToMailboxAddresses = [new() { Address = usuarioExistente.Email!, Name = usuarioExistente.NombreADesplegar! }],
                        Body = emailBody,
                        BodyType = EnumEmailBodyType.HTML_BODY,
                        Subject = "ANID: Validación Registro Usuario"
                    };

                    var result = await emailSender.SendEmail(email);

                    CommandResponse.Data = JsonConvert.SerializeObject(new EmailConfirmationTokenViewModel()
                    {
                        Email = usuarioExistente.Email!,
                        ConfirmationToken = confirmationEmailToken
                    });
                    CommandResponse.Result = true;
                }
                else
                {
                    CommandResponse.Data = JsonConvert.SerializeObject(new ResponseSingleId() { Id = id_Usuario });
                    CommandResponse.Result = true;
                }

                await transaction.CommitAsync(cancellationToken);
                return CommandResponse;
            }

            var existeEntidad = await entidadRepository.BuscarPor_Id_Usuario_Id_UnidadOrganizacional(Guid.Parse(usuarioExistente.Id), Guid.Empty);

            if (existeEntidad is not null)
            {
                AddError("Ya existe la entidad base para el usuario, al ser un usuario nuevo esta entidad no deberia existir, se cancela la creacion del usuario");
                await transaction.RollbackAsync(cancellationToken);
                return CommandResponse;
            }

            var baseEntityCommand = new BaseEntityCommand()
            {
                CodigoOrganizacion = JsonConvert.DeserializeObject<InformacionAdicionalModel>(usuarioExistente.InformacionAdicional!)!.NumeroDeDocumento,
                NombreOrganizacion = nombreADesplegar,
                Id_Usuario = Guid.Parse(usuarioExistente.Id),
                TipoDeEntidad = EnumTipoDeEntidad.PERSONA,
                CorreoElectronico = usuarioExistente.Email
            };

            var resulCrearBaseEntityCommand = await mediator.SendCommand(baseEntityCommand, cancellationToken);

            if (!resulCrearBaseEntityCommand.Result)
            {
                foreach (var item in resulCrearBaseEntityCommand.ValidationResult.Errors)
                {
                    AddError($"{item.ErrorCode} {item.ErrorMessage}");
                }
                await transaction.RollbackAsync(cancellationToken);
                return CommandResponse;
            }

            if (string.IsNullOrEmpty(resulCrearBaseEntityCommand.Data))
            {
                AddError("No se pudo crear la entidad base del usuario");
                await transaction.RollbackAsync(cancellationToken);
                return CommandResponse;
            }

            await transaction.CommitAsync(cancellationToken);

            CommandResponse.Data = string.Empty;
            CommandResponse.Result = true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            AddError($"Error no manejado al momento de crear un usuario, error: {ex.Message}");
            return CommandResponse;
        }

        return CommandResponse;
    }
}