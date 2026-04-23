using AUT2Services.Domain.Core.Enumerations;
using AUT2Services.Domain.Core.Models;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace AUT2Services.Infra.Security.Services;

public sealed class PasswordChangeChallengeMessageService(
    IOptions<SendEmailOptions> sendEmailOptions,
    IOptions<PasswordChangeOptions> passwordChangeOptions) : IPasswordChangeChallengeMessageService
{
    private readonly SendEmailOptions sendEmailOptions = sendEmailOptions.Value;
    private readonly PasswordChangeOptions passwordChangeOptions = passwordChangeOptions.Value;

    public PasswordChangeChallengeDispatch CreateDispatch(Usuario usuario, DateTimeOffset expiresAtUtc)
    {
        var code = GenerateCode(passwordChangeOptions.CodeLength);
        var codeHash = ComputeCodeHash(usuario.Id, code);
        var emailMessage = BuildEmailMessage(usuario, code, expiresAtUtc);
        return new PasswordChangeChallengeDispatch(code, codeHash, emailMessage);
    }

    public bool IsCodeMatch(string userId, string code, string expectedHash)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(expectedHash))
        {
            return false;
        }

        var computedHash = ComputeCodeHash(userId, code);
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(computedHash),
            Encoding.UTF8.GetBytes(expectedHash));
    }

    private EmailDataModel BuildEmailMessage(Usuario usuario, string code, DateTimeOffset expiresAtUtc)
    {
        var body = $@"
<!DOCTYPE html>
<html lang=""es"">
  <head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Código de validación para cambio de clave</title>
  </head>
  <body style=""margin:0;padding:0;font-family:Arial,sans-serif;background-color:#f4f4f4;"">
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
      <tr>
        <td align=""center"" style=""padding:20px 0;"">
          <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""background-color:#ffffff;border-radius:8px;overflow:hidden;box-shadow:0 4px 6px rgba(0,0,0,0.1);"">
            <tr>
              <td align=""center"" style=""padding:24px 24px 8px 24px;"">
                <h1 style=""color:#333333;margin:0;font-size:24px;"">Agencia Nacional de Investigación y Desarrollo</h1>
              </td>
            </tr>
            <tr>
              <td style=""padding:24px;text-align:center;"">
                <h2 style=""color:#333333;margin:0 0 16px 0;font-size:22px;"">Hola {usuario.NombreADesplegar ?? "Usuario"}</h2>
                <p style=""color:#555555;line-height:1.6;margin:0 0 24px 0;"">
                  Recibimos una solicitud para cambiar tu clave de acceso. Usa el siguiente código de validación:
                </p>
                <div style=""margin:0 0 24px 0;padding:16px;background-color:#f8f9fa;border:1px dashed #d52b1e;word-break:break-all;font-family:Consolas,monospace;font-size:24px;color:#d52b1e;letter-spacing:4px;font-weight:bold;"">
                  {code}
                </div>
                <p style=""color:#555555;line-height:1.6;margin:0;"">
                  Este código expira el {expiresAtUtc:dd-MM-yyyy HH:mm} UTC.
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

        return new EmailDataModel
        {
            FromMailboxAddresses = [new() { Address = sendEmailOptions.Remitente, Name = "Correo ANID" }],
            ToMailboxAddresses = [new() { Address = usuario.Email ?? string.Empty, Name = usuario.NombreADesplegar ?? string.Empty }],
            Body = body,
            BodyType = EnumEmailBodyType.HTML_BODY,
            Subject = "ANID: Código de validación para cambio de clave"
        };
    }

    private static string GenerateCode(int codeLength)
    {
        var buffer = new char[codeLength];
        for (var i = 0; i < buffer.Length; i++)
        {
            buffer[i] = (char)('0' + RandomNumberGenerator.GetInt32(0, 10));
        }

        return new string(buffer);
    }

    private static string ComputeCodeHash(string userId, string code)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes($"{userId}:{code}"));
        return Convert.ToHexString(bytes);
    }
}
