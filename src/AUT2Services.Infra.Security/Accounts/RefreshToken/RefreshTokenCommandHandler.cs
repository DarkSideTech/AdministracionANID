using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Time;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AUT2Services.Infra.Security.Accounts.RefreshToken;

public class RefreshTokenCommandHandler : CommandHandler,
    IRequestHandler<RefreshTokenCommand, CommandResponse>
{
    private readonly UserManager<Usuario> userManager;
    private readonly ITokenService tokenService;
    private readonly ISecurityRepository securityRepository;
    private readonly IMediatorHandler mediator;
    private readonly IUserAccessor userAccessor;
    private readonly IServicioDeDominioRepository servicioDeDominioRepository;
    private readonly ICsrfService csrfService;
    private readonly AUT2ServicesContext aUT2ServicesContext;
    private readonly IAuthCookieService authCookieService;
    private readonly ICurrentUserService currentUserService;
    private readonly IClock clock;

    public RefreshTokenCommandHandler(
        UserManager<Usuario> userManager,
        ITokenService tokenService,
        ISecurityRepository securityRepository,
        IMediatorHandler mediator,
        IUserAccessor userAccessor,
        IServicioDeDominioRepository servicioDeDominioRepository,
        ICsrfService csrfService,
        AUT2ServicesContext aUT2ServicesContext,
        IAuthCookieService authCookieService,
        ICurrentUserService currentUserService,
        IClock clock)
    {
        this.userManager = userManager;
        this.tokenService = tokenService;
        this.securityRepository = securityRepository;
        this.mediator = mediator;
        this.userAccessor = userAccessor;
        this.servicioDeDominioRepository = servicioDeDominioRepository;
        this.csrfService = csrfService;
        this.aUT2ServicesContext = aUT2ServicesContext;
        this.authCookieService = authCookieService;
        this.currentUserService = currentUserService;
        this.clock = clock;
    }

    public async Task<CommandResponse> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;

        if (!command.IsValid())
        {
            return CommandResponse;
        }

        AccessTokenResult? accessToken = null!;
        Usuario? usuario = null!;
        Domain.Security.Entities.RefreshToken? existingRefreshToken = null!;

        try
        {
            if (!csrfService.IsRequestValid(command.Request))
            {
                AddError("Invalid CSRF token.");
                return CommandResponse;
            }

            if (!command.Request.Cookies.TryGetValue(EnumAuthCookieNames.RefreshToken, out var tokenValue))
            {
                AddError("No se encuentra el RefreshToken.");
                return CommandResponse;
            }

            var tokenHash = tokenService.HashRefreshToken(tokenValue);
            existingRefreshToken = await aUT2ServicesContext.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken: cancellationToken);

            if (existingRefreshToken is null)
            {
                authCookieService.ClearAuthCookies(command.Response);
                AddError("Token de actualización no válido.");
                return CommandResponse;
            }

            if (existingRefreshToken.IsExpired(clock))
            {
                authCookieService.ClearAuthCookies(command.Response);
                AddError("RefreshToken ha expirado.");
                return CommandResponse;
            }

            if (existingRefreshToken.RevokedAtUtc is not null)
            {
                if (string.Equals(existingRefreshToken.RevocationReason, EnumRefreshTokenRevocationReasons.Rotated, StringComparison.Ordinal))
                {
                    await tokenService.RevokeSessionAsync(existingRefreshToken.SessionId, EnumRefreshTokenRevocationReasons.ReuseDetected);
                    authCookieService.ClearAuthCookies(command.Response);
                    csrfService.EnsureTokenCookie(command.Response);
                    AddError("Se ha detectado la reutilización del token de actualización. Sesión revocada.");
                    return CommandResponse;
                }

                authCookieService.ClearAuthCookies(command.Response);
                AddError("RefreshToken no válido.");
                return CommandResponse;
            }

            if (existingRefreshToken.User is null)
            {
                authCookieService.ClearAuthCookies(command.Response);
                AddError("RefreshToken no válido, No Existe.");
                return CommandResponse;
            }

            usuario = existingRefreshToken.User;

            if (!existingRefreshToken.User.EmailConfirmed)
            {
                await tokenService.RevokeSessionAsync(existingRefreshToken.SessionId, EnumRefreshTokenRevocationReasons.Logout);
                authCookieService.ClearAuthCookies(command.Response);
                csrfService.EnsureTokenCookie(command.Response);
                AddError("Debes confirmar tu correo electrónico antes de actualizar la sesión.");
                return CommandResponse;
            }

            var persistedSelection = SelectedOrganizationSessionSerializer.Parse(existingRefreshToken.SelectedOrganization);
            var hasSelectedOrganizationForRotation = existingRefreshToken.Id_Entidad.HasValue
                && !string.IsNullOrWhiteSpace(persistedSelection.OrganizationCode);
            var idEntidadForRotation = existingRefreshToken.Id_Entidad;
            var selectedContextForRotation = hasSelectedOrganizationForRotation
                ? await currentUserService.GetSelectedSessionContextAsync(
                    idEntidadForRotation!.Value,
                    persistedSelection.IdRol,
                    cancellationToken)
                : null;
            var idRolSeleccionadoForRotation = persistedSelection.IdRol ?? selectedContextForRotation?.EntidadRolSeleccionado?.Id_Rol;

            var rotatedRefreshToken = tokenService.CreateRefreshToken(
                existingRefreshToken.SessionId,
                SelectedOrganizationSessionSerializer.Serialize(
                    selectedContextForRotation?.CodigoOrganizacionSeleccionada ?? persistedSelection.OrganizationCode,
                    idRolSeleccionadoForRotation));
            existingRefreshToken.RevokedAtUtc = clock.UtcNow;
            existingRefreshToken.RevocationReason = EnumRefreshTokenRevocationReasons.Rotated;
            existingRefreshToken.ReplacedByTokenHash = rotatedRefreshToken.RefreshToken.TokenHash;

            rotatedRefreshToken.RefreshToken.UserId = existingRefreshToken.UserId;
            rotatedRefreshToken.RefreshToken.Id_Entidad = existingRefreshToken.Id_Entidad;
            aUT2ServicesContext.RefreshTokens.Add(rotatedRefreshToken.RefreshToken);
            await aUT2ServicesContext.SaveChangesAsync();

            accessToken = string.IsNullOrWhiteSpace(persistedSelection.OrganizationCode)
                ? await tokenService.GenerateAccessTokenAsync(
                    existingRefreshToken.User,
                    existingRefreshToken.SessionId)
                : await tokenService.GenerateAccessTokenAsync(
                    existingRefreshToken.User,
                    existingRefreshToken.SessionId,
                    existingRefreshToken.Id_Entidad,
                    idRolSeleccionadoForRotation);

            authCookieService.AppendAuthCookies(
                command.Response,
                accessToken,
                rotatedRefreshToken.RefreshToken.ExpiresAtUtc,
                rotatedRefreshToken.PlainTextToken);

            csrfService.EnsureTokenCookie(command.Response);
        }
        catch (Exception ex)
        {
            AddError($"Error al momento de validar y generar el refreshtoken, message [{ex.Message}]");
            CommandResponse.Data = string.Empty;
            CommandResponse.Result = false;
            return CommandResponse;
        }

        var persistedSessionSelection = SelectedOrganizationSessionSerializer.Parse(existingRefreshToken.SelectedOrganization);
        var hasSelectedOrganization = existingRefreshToken.Id_Entidad.HasValue
            && !string.IsNullOrWhiteSpace(persistedSessionSelection.OrganizationCode);
        var idEntidad = existingRefreshToken.Id_Entidad;
        var selectedContext = hasSelectedOrganization
            ? await currentUserService.GetSelectedSessionContextAsync(
                idEntidad!.Value,
                persistedSessionSelection.IdRol,
                cancellationToken)
            : null;
        var idRolSeleccionado = persistedSessionSelection.IdRol ?? selectedContext?.EntidadRolSeleccionado?.Id_Rol;

        var procesosActivos = hasSelectedOrganization && idRolSeleccionado.HasValue
            ? await currentUserService.GetProcesosActivosPorEntidadAsync(
                idEntidad!.Value,
                idRolSeleccionado.Value,
                cancellationToken)
            : null;

        CommandResponse.Data = new ProfileLogin(
            AccessTokenExpiracion: accessToken.ExpiresAtUtc,
            OrganizacionesPorUsuario: await tokenService.BuscarOrganizacionesPorIdUsuario(usuario.Id),
            UnidadesOrganizacionalesPorUsuario: hasSelectedOrganization
                ? await currentUserService.GetUnidadesOrganizacionalesEntidadRolPorUsuarioAsync(
                    idEntidad!.Value,
                    Guid.Parse(usuario.Id),
                    cancellationToken)
                : [],
            User: await tokenService.CreateUserDtoAsync(
                existingRefreshToken.User!,
                idEntidad!.Value,
                selectedContext?.EntidadRolSeleccionado),
            ProcesosActivos: procesosActivos,
            CodigoOrganizacionSeleccionada: selectedContext?.CodigoOrganizacionSeleccionada ?? persistedSessionSelection.OrganizationCode,
            NombreOrganizacionSeleccionada: selectedContext?.NombreOrganizacionSeleccionada,
            CodigoUnidadOrganizacionalSeleccionada: selectedContext?.CodigoUnidadOrganizacionalSeleccionada,
            NombreUnidadOrganizacionalSeleccionada: selectedContext?.NombreUnidadOrganizacionalSeleccionada,
            IdEntidadSeleccionada: hasSelectedOrganization ? idEntidad!.Value.ToString() : null,
            EntidadRolSeleccionado: selectedContext?.EntidadRolSeleccionado,
            SeleccionOrganizacionRequerida: string.IsNullOrWhiteSpace(persistedSessionSelection.OrganizationCode)
        );
        CommandResponse.Result = true;

        return CommandResponse;
    }
}
