using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Enumerations;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Domain.Core.Models;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Accounts.BaseEntity;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Infra.Security.Services;
using AUT2Services.Infra.Security.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
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
    private readonly ICsrfService csrfService;
    private readonly SendEmailOptions sendEmailOptions;
    private readonly JwtOptions jwtOptions;

    public RegisterCommandHandler(
        UserManager<Usuario> userManager,
        IMediatorHandler mediator,
        AUT2ServicesContext aUT2ServicesContext,
        IConfiguration configuration,
        IEntidadRepository entidadRepository,
        IOptions<SendEmailOptions> sendEmailOptions,
        IEmailMessageSender emailSender,
        IOptions<JwtOptions> jwtOptions,
        ICsrfService csrfService)
    {
        this.userManager = userManager;
        this.configuration = configuration;
        this.aUT2ServicesContext = aUT2ServicesContext;
        this.mediator = mediator;
        this.entidadRepository = entidadRepository;
        this.emailSender = emailSender;
        this.csrfService = csrfService;
        this.sendEmailOptions = sendEmailOptions.Value;
        this.jwtOptions = jwtOptions.Value;
    }

    public async Task<CommandResponse> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;

        if (!command.IsValid())
        {
            return CommandResponse;
        }

        if (!csrfService.IsRequestValid(command.Request))
        {
            AddError("Invalid CSRF token.");
            return CommandResponse;
        }

        if (command.Contraseña != command.ConfirmaContraseña)
        {
            AddError("La contraseña no corresponde.");
            return CommandResponse;
        }

        var existingUser = await userManager.FindByEmailAsync(command.CorreoElectronico!);
        if (existingUser is not null)
        {
            AddError("Correo electrónico ya registrado.");
            return CommandResponse;
        }

        Usuario? usuarioExistente = null;
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
            AccessFailedCount = jwtOptions.MaximaCantidadIntentosFallidos,
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

        using var transaction = await aUT2ServicesContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var usuarioNuevo = await userManager.CreateAsync(nuevoUsuario, command.Contraseña!);
            if (!usuarioNuevo.Succeeded)
            {
                foreach (var item in usuarioNuevo.Errors)
                {
                    AddError($"{item.Code} - {item.Description}");
                }
                await transaction.RollbackAsync(cancellationToken);
                return CommandResponse;
            }

            usuarioExistente = await userManager.FindByEmailAsync(command.CorreoElectronico);
            if (usuarioExistente == null)
            {
                AddError("El usuario buscado no existe, no se puede seguir con el registro del usuario");
                await transaction.RollbackAsync(cancellationToken);
                return CommandResponse;
            }

            if (usuarioExistente.EmailConfirmed.Equals(false) ||
                usuarioExistente.RequiereValidacionEnrrolamiento.Equals(true))
            {
                if (!usuarioExistente.EmailConfirmed)
                {
                    var emailToken = await this.userManager.GenerateEmailConfirmationTokenAsync(usuarioExistente);
                    var confirmationEmailToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailToken));
                    var confirmationId = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(usuarioExistente.Id));

                    Console.WriteLine($"confirmationEmailToken: {confirmationEmailToken}, id: {confirmationId}");
                    if (string.IsNullOrEmpty(confirmationEmailToken))
                    {
                        AddError("No es posible generar el token de confirmacion del correo electronico del usuario");
                        await transaction.RollbackAsync(cancellationToken);
                        return CommandResponse;
                    }

                    var emailConfirmationUrl = QueryHelpers.AddQueryString(sendEmailOptions.APIValidateEmail, new Dictionary<string, string?>
                    {
                        ["userId"] = confirmationId,
                        ["token"] = confirmationEmailToken
                    });

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
										                        <a href=""{sendEmailOptions.APIValidateEmail}?validationtoken={emailConfirmationUrl}"">Link para Validar Cuenta de Correo</a>. 
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
                        Id = confirmationId!,
                        ConfirmationToken = confirmationEmailToken
                    });
                    CommandResponse.Result = true;
                }
                else
                {
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

        CommandResponse.Data = JsonConvert.SerializeObject(new RegisterResponse(
            Email: usuarioExistente.Email ?? command.CorreoElectronico,
            RequiresEmailConfirmation: usuarioExistente.TipoDeUsuario!.Equals(EnumTipoDeUsuario.NACIONAL),
            Message: "Usuario registrado. Confirma tu correo electrónico antes de iniciar sesión.",
            ConfirmationUrl: sendEmailOptions.APIValidateEmail
        ));
        CommandResponse.Result = true;
        return CommandResponse;
    }
}