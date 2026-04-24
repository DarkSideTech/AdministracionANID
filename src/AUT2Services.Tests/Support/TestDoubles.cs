using AUT2Services.Domain.Core.Models;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AUT2Services.Tests.Support;

internal sealed class StubCsrfService(bool isValid = true) : ICsrfService
{
    public void EnsureTokenCookie(HttpResponse response, string? existingToken = null)
    {
    }

    public bool IsRequestValid(HttpRequest request)
    {
        return isValid;
    }
}

internal sealed class StubMediatorHandler : IMediatorHandler
{
    public Task PublishEvent<T>(T @event) where T : Event
    {
        return Task.CompletedTask;
    }

    public Task PublishEvent<T>(T @event, CancellationToken cancellationToken) where T : Event
    {
        return Task.CompletedTask;
    }

    public Task<CommandResponse> SendCommand<T>(T command) where T : Command
    {
        return Task.FromResult(new CommandResponse { Result = true });
    }

    public Task<CommandResponse> SendCommand<T>(T command, CancellationToken cancellationToken) where T : Command
    {
        return Task.FromResult(new CommandResponse { Result = true });
    }
}

internal sealed class StubEmailConfirmationThrottleService(bool canSend = true) : IEmailConfirmationThrottleService
{
    public bool CanSend(string email, out DateTimeOffset? nextAllowedAtUtc)
    {
        nextAllowedAtUtc = null;
        return canSend;
    }
}

internal sealed class StubEmailConfirmationMessageService : IEmailConfirmationMessageService
{
    public Task<EmailConfirmationDispatch> CreateDispatchAsync(Usuario usuario)
    {
        return Task.FromResult(new EmailConfirmationDispatch(
            usuario.Email ?? string.Empty,
            "encoded-user",
            "encoded-token",
            "validation-token",
            "http://localhost/auto",
            "http://localhost/manual"));
    }

    public EmailDataModel BuildEmailMessage(Usuario usuario, EmailConfirmationDispatch dispatch)
    {
        return new EmailDataModel
        {
            Subject = "Validacion",
            Body = "Body",
            FromMailboxAddresses = [new AddressMailbox { Address = "sender@example.com", Name = "Sender" }],
            ToMailboxAddresses = [new AddressMailbox { Address = usuario.Email ?? string.Empty, Name = usuario.NombreADesplegar ?? string.Empty }]
        };
    }

    public string? GetValidationTokenForResponse(EmailConfirmationDispatch dispatch)
    {
        return dispatch.ValidationToken;
    }
}

internal sealed class StubPasswordChangeChallengeMessageService(
    string code = "123456",
    string hash = "TEST_HASH") : IPasswordChangeChallengeMessageService
{
    public PasswordChangeChallengeDispatch CreateDispatch(Usuario usuario, DateTimeOffset expiresAtUtc)
    {
        return new PasswordChangeChallengeDispatch(
            code,
            hash,
            new EmailDataModel
            {
                Subject = "Cambio de clave",
                Body = $"Codigo: {code}",
                FromMailboxAddresses = [new AddressMailbox { Address = "sender@example.com", Name = "Sender" }],
                ToMailboxAddresses = [new AddressMailbox { Address = usuario.Email ?? string.Empty, Name = usuario.NombreADesplegar ?? string.Empty }]
            });
    }

    public PasswordChangeChallengeDispatch CreateRecoveryDispatch(Usuario usuario, DateTimeOffset expiresAtUtc)
    {
        return CreateDispatch(usuario, expiresAtUtc);
    }

    public bool IsCodeMatch(string userId, string inputCode, string expectedHash)
    {
        return string.Equals(inputCode, code, StringComparison.Ordinal)
            && string.Equals(expectedHash, hash, StringComparison.Ordinal);
    }
}

internal sealed class StubEmailMessageSender(ResultModel result) : IEmailMessageSender
{
    public Task<ResultModel> SendEmail(EmailDataModel emailData)
    {
        return Task.FromResult(result);
    }
}

internal sealed class StubTicketDataSender(ResultModel result) : ITicketDataSender
{
    public Task<ResultModel> SendTicketData(TicketDataModel ticketDataModel)
    {
        return Task.FromResult(result);
    }
}

internal sealed class StubSessionValidationService(bool isValid = true) : ISessionValidationService
{
    public Task<bool> IsSessionValidAsync(string? userId, string? sessionId, string? securityStamp, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(isValid);
    }
}

internal sealed class StubTokenService : ITokenService
{
    public List<(string SessionId, string Reason)> RevokedSessions { get; } = [];

    public Task<AccessTokenResult> GenerateAccessTokenAsync(Usuario user, string sessionId, Guid? idEntidad = null, Guid? idRol = null)
    {
        throw new NotSupportedException();
    }

    public RefreshTokenIssuanceResult CreateRefreshToken(string sessionId, string? selectedOrganization = null)
    {
        throw new NotSupportedException();
    }

    public Task<IList<OrganizacionPorUsuario>> BuscarOrganizacionesPorIdUsuario(string idUsuario)
    {
        throw new NotSupportedException();
    }

    public string HashRefreshToken(string refreshToken)
    {
        throw new NotSupportedException();
    }

    public Task<UserDto> CreateUserDtoAsync(Usuario user, Guid? id_Entidad = null, EntidadRolSeleccionado? entidadRolSeleccionado = null)
    {
        throw new NotSupportedException();
    }

    public Task RevokeSessionAsync(string sessionId, string reason)
    {
        RevokedSessions.Add((sessionId, reason));
        return Task.CompletedTask;
    }
}

internal sealed class StubAuthCookieService : IAuthCookieService
{
    public bool Cleared { get; private set; }

    public void AppendAuthCookies(HttpResponse response, AccessTokenResult accessToken, DateTimeOffset? refreshTokenExpiresAtUtc, string refreshToken)
    {
    }

    public void ClearAuthCookies(HttpResponse response)
    {
        Cleared = true;
    }
}

internal sealed class StubCurrentUserService(
    bool isAuthenticated = true,
    string? userId = null,
    string? sessionId = "test-session-id",
    ClaimsPrincipal? principal = null) : ICurrentUserService
{
    public bool IsAuthenticated { get; } = isAuthenticated;
    public string? UserId { get; } = userId;
    public string? SessionId { get; } = sessionId;

    public Task<Usuario?> GetUserAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<Usuario?>(null);
    }

    public Task<IList<ProcesoActivo>?> GetProcesosActivosAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IList<ProcesoActivo>?>([]);
    }

    public Task<IList<ProcesoActivo>?> GetProcesosActivosPorEntidadAsync(Guid idEntidad, Guid idRol, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IList<ProcesoActivo>?>([]);
    }

    public Task<IList<UnidadOrganizacionalEntidadRolPorUsuario>> GetUnidadesOrganizacionalesEntidadRolPorUsuarioAsync(Guid idEntidad, Guid idUsuario, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IList<UnidadOrganizacionalEntidadRolPorUsuario>>([]);
    }

    public Task<SelectedSessionContext?> GetSelectedSessionContextAsync(Guid idEntidad, Guid? idRol = null, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<SelectedSessionContext?>(null);
    }

    public IList<ProcesoActivo>? GetProcesosActivos(IEnumerable<Claim>? claims)
    {
        return [];
    }

    public ClaimsPrincipal? GetClaimsPrincipal(CancellationToken cancellationToken = default)
    {
        return principal;
    }

    public Task<CurrentUserResponse> GetCurrentUserResponseAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new CurrentUserResponse(false, null, null, null, null, [], [], [], null, null, null, null, null, null, null, true));
    }
}
