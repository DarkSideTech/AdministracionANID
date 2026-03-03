using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AUT2Services.Infra.Security.Accounts.LoginOrganizacion;

public class LoginOrganizacionCommandHandler : CommandHandler,
    IRequestHandler<LoginOrganizacionCommand, CommandResponse>
{
    private readonly UserManager<Usuario> userManager;
    private readonly JwtOptions jwtOptions;
    private readonly ISecurityRepository securityRepository;
    private readonly ITokenService tokenService;
    private readonly IOrganizacionRepository organizacionRepository;
    private readonly IUnidadOrganizacionalRepository unidadOrganizacionalRepository;

    public LoginOrganizacionCommandHandler(
        ISecurityRepository securityRepository,
        UserManager<Usuario> userManager,
        IOptions<JwtOptions> jwtOptions,
        ITokenService tokenService,
        IOrganizacionRepository organizacionRepository,
        IUnidadOrganizacionalRepository unidadOrganizacionalRepository)
    {
        this.securityRepository = securityRepository;
        this.userManager = userManager;
        this.jwtOptions = jwtOptions.Value;
        this.tokenService = tokenService;
        this.organizacionRepository = organizacionRepository;
        this.unidadOrganizacionalRepository = unidadOrganizacionalRepository;
    }

    public async Task<CommandResponse> Handle(LoginOrganizacionCommand command, CancellationToken cancellationToken)
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
            var user = await userManager.Users
                .FirstOrDefaultAsync(x => x.Email == command.Email!);

            if (user is null)
            {
                AddError("Usuario o clave no corresponden");
                return CommandResponse;
            }

            var resultado = await userManager
                .CheckPasswordAsync(user, command.Password!);

            if (!resultado)
            {
                AddError("Usuario o clave no corresponden");
                return CommandResponse;
            }

            Entidad entidad = null;
            Organizacion organizacion = null;

            if (string.IsNullOrEmpty(command.Organizacion))
            {
                AddError("Debe ingresar una organizacion valida");
                return CommandResponse;
            }
            else
            {
                organizacion = await organizacionRepository.BuscarPor_Codigo(command.Organizacion!);

                if (organizacion is null)
                {
                    AddError("Usuario o clave no corresponden");
                    return CommandResponse;
                }

                entidad = await securityRepository.BuscarEntidadPrincipalPorUsuarioOrganizacion(Guid.Parse(user.Id), organizacion.Id);
            }

            if (entidad is null)
            {
                AddError("Usuario o clave no corresponden");
                return CommandResponse;
            }

            var unidadorganizacional = await unidadOrganizacionalRepository.BuscarPor_Id(entidad.Id_UnidadOrganizacional);

            if (unidadorganizacional is null)
            {
                AddError("Usuario o clave no corresponden");
                return CommandResponse;
            }

            var (jwtToken, expirationDateInUtc) = await tokenService.GenerateJwtTokenLoginOrganizacion(user, entidad.Id);
            var refreshTokenValue = tokenService.GenerateRefreshToken();

            var refreshTokenExpirationDateInUtc = DateTime.UtcNow.AddMinutes(this.jwtOptions.ExpirationRefreshTokenTimeInMinutes);

            user.RefreshToken = refreshTokenValue;
            user.RefreshTokenExpiresAtUtc = refreshTokenExpirationDateInUtc;

            await userManager.UpdateAsync(user);

            tokenService.WriteAuthTokenAsHttpOnlyCookie(EnumAuthCookie.ACCESS_TOKEN, jwtToken, expirationDateInUtc);
            tokenService.WriteAuthTokenAsHttpOnlyCookie(EnumAuthCookie.REFRESH_TOKEN, user.RefreshToken, refreshTokenExpirationDateInUtc);

            profile = new ProfileModel
            {
                AccessToken = jwtToken,
                RefreshToken = refreshTokenValue
            };

        }
        catch (Exception ex)
        {
            AddError($"Error al momento de obtener los datos del usuario, message [{ex.Message}]");
            CommandResponse.Data = string.Empty;
            CommandResponse.Result = false;
        }

        CommandResponse.Data = JsonConvert.SerializeObject(profile);
        CommandResponse.Result = true;

        return CommandResponse;
    }
}
