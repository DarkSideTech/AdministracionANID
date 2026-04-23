using AUT2Services.Domain.Core.Enumerations;
using AUT2Services.Domain.Core.Models;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace AUT2Services.Infra.Security.Services;

public sealed class EmailConfirmationMessageService(
    UserManager<Usuario> userManager,
    IOptions<SendEmailOptions> sendEmailOptions,
    IOptions<EmailValidationOptions> emailValidationOptions,
    IHostEnvironment hostEnvironment) : IEmailConfirmationMessageService
{
    private readonly UserManager<Usuario> userManager = userManager;
    private readonly SendEmailOptions sendEmailOptions = sendEmailOptions.Value;
    private readonly EmailValidationOptions emailValidationOptions = emailValidationOptions.Value;
    private readonly IHostEnvironment hostEnvironment = hostEnvironment;

    public async Task<EmailConfirmationDispatch> CreateDispatchAsync(Usuario usuario)
    {
        var emailToken = await userManager.GenerateEmailConfirmationTokenAsync(usuario);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailToken));
        var encodedUserId = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(usuario.Id));
        var validationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(new ConfirmEmailRequest(encodedUserId, encodedToken))));

        return new EmailConfirmationDispatch(
            Email: usuario.Email ?? string.Empty,
            EncodedUserId: encodedUserId,
            EncodedToken: encodedToken,
            ValidationToken: validationToken,
            AutoConfirmationUrl: BuildConfirmationUrl(usuario.Email, validationToken),
            ManualConfirmationUrl: BuildConfirmationUrl(usuario.Email, null));
    }

    public EmailDataModel BuildEmailMessage(Usuario usuario, EmailConfirmationDispatch dispatch)
    {
        var body = $@"
<!DOCTYPE html>
<html lang=""es"">
  <head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Validacion de correo</title>
  </head>
  <body style=""margin:0;padding:0;font-family:Arial,sans-serif;background-color:#f4f4f4;"">
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
      <tr>
        <td align=""center"" style=""padding:20px 0;"">
          <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""background-color:#ffffff;border-radius:8px;overflow:hidden;box-shadow:0 4px 6px rgba(0,0,0,0.1);"">
            <tr>
              <td align=""center"" style=""padding:24px 24px 8px 24px;"">
                <h1 style=""color:#333333;margin:0;font-size:24px;"">Agencia Nacional de Investigacion y Desarrollo</h1>
              </td>
            </tr>
            <tr>
              <td style=""padding:24px;text-align:center;"">
                <h2 style=""color:#333333;margin:0 0 16px 0;font-size:22px;"">Hola {usuario.NombreADesplegar ?? "Nuevo Usuario"}</h2>
                <p style=""color:#555555;line-height:1.6;margin:0 0 24px 0;"">
                  Tu cuenta fue creada correctamente. Para finalizar el proceso debes validar tu correo electronico.
                </p>
                <p style=""margin:0 0 24px 0;"">
                  <a href=""{dispatch.AutoConfirmationUrl}"" style=""display:inline-block;padding:14px 24px;background-color:#d52b1e;color:#ffffff;text-decoration:none;border-radius:6px;font-weight:bold;"">
                    Validar correo ahora
                  </a>
                </p>
                <p style=""color:#555555;line-height:1.6;margin:0 0 16px 0;"">
                  Si prefieres validar manualmente, abre la pantalla de validacion y pega el siguiente token:
                </p>
                <div style=""margin:0 0 24px 0;padding:16px;background-color:#f8f9fa;border:1px dashed #007bff;word-break:break-all;font-family:Consolas,monospace;font-size:13px;color:#0d6efd;"">
                  {dispatch.ValidationToken}
                </div>
                <p style=""color:#555555;line-height:1.6;margin:0;"">
                  Pantalla de validacion manual:
                  <a href=""{dispatch.ManualConfirmationUrl}"" style=""color:#0d6efd;text-decoration:none;"">{dispatch.ManualConfirmationUrl}</a>
                </p>
              </td>
            </tr>
            <tr>
              <td style=""padding:12px;background-color:#333333;color:#ffffff;text-align:center;font-size:12px;"">
                <p style=""margin:0;"">&copy; 2026 Dark Side Tech. Todos los derechos reservados.</p>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </body>
</html>";

        return new EmailDataModel()
        {
            FromMailboxAddresses = [new() { Address = sendEmailOptions.Remitente, Name = "Correo ANID" }],
            ToMailboxAddresses = [new() { Address = usuario.Email ?? string.Empty, Name = usuario.NombreADesplegar ?? string.Empty }],
            Body = body,
            BodyType = EnumEmailBodyType.HTML_BODY,
            Subject = "ANID: Validacion Registro Usuario",
            ValidationToken = dispatch.ValidationToken
        };
    }

    public string? GetValidationTokenForResponse(EmailConfirmationDispatch dispatch)
    {
        return hostEnvironment.IsDevelopment() && emailValidationOptions.ExposeConfirmationUrlInDevelopment
            ? dispatch.ValidationToken
            : null;
    }

    private string BuildConfirmationUrl(string? email, string? validationToken)
    {
        var confirmationUrlBase = emailValidationOptions.ConfirmationUrlBase?.Trim();
        if (string.IsNullOrWhiteSpace(confirmationUrlBase))
        {
            return string.Empty;
        }

        var queryString = new Dictionary<string, string?>();

        if (!string.IsNullOrWhiteSpace(email))
        {
            queryString["email"] = email;
        }

        if (!string.IsNullOrWhiteSpace(validationToken))
        {
            queryString["validationToken"] = validationToken;
        }

        return QueryHelpers.AddQueryString(confirmationUrlBase, queryString);
    }
}
