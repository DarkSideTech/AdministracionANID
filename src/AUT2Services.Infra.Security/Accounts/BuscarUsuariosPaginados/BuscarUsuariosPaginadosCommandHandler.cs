using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Infra.Security.Accounts.BuscarUsuariosPaginados;

public class BuscarUsuariosPaginadosCommandHandler(
    UserManager<Usuario> userManager,
    ICurrentUserService currentUserService,
    ISessionValidationService sessionValidationService) : CommandHandler,
    IRequestHandler<BuscarUsuariosPaginadosCommand, CommandResponse>
{
    private readonly UserManager<Usuario> userManager = userManager;
    private readonly ICurrentUserService currentUserService = currentUserService;
    private readonly ISessionValidationService sessionValidationService = sessionValidationService;

    public async Task<CommandResponse> Handle(BuscarUsuariosPaginadosCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;

        if (!command.IsValid())
        {
            return CommandResponse;
        }

        if (!currentUserService.IsAuthenticated || string.IsNullOrWhiteSpace(currentUserService.UserId))
        {
            AddError("Usuario no autorizado.");
            return CommandResponse;
        }

        var principal = currentUserService.GetClaimsPrincipal(cancellationToken);
        var securityStamp = principal?.FindFirst(EnumTokenValidationClaims.SecurityStamp)?.Value;
        var isSessionValid = await sessionValidationService.IsSessionValidAsync(
            currentUserService.UserId,
            currentUserService.SessionId,
            securityStamp,
            cancellationToken);

        if (!isSessionValid)
        {
            AddError("La sesion del usuario no es valida.");
            return CommandResponse;
        }

        var numeroDePagina = command.NumeroDePagina.GetValueOrDefault(1);
        var cantidadPorPagina = command.CantidadPorPagina.GetValueOrDefault(10);
        var busqueda = command.Busqueda?.Trim();

        var query = userManager.Users
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var busquedaNormalizada = busqueda.ToUpperInvariant();
            var incluyeActivos = MatchesBooleanSearch(busquedaNormalizada, true);
            var incluyeInactivos = MatchesBooleanSearch(busquedaNormalizada, false);

            query = query.Where(usuario =>
                (usuario.Id ?? string.Empty).ToUpper().Contains(busquedaNormalizada) ||
                (usuario.Email ?? string.Empty).ToUpper().Contains(busquedaNormalizada) ||
                (usuario.PhoneNumber ?? string.Empty).ToUpper().Contains(busquedaNormalizada) ||
                (usuario.Descripcion ?? string.Empty).ToUpper().Contains(busquedaNormalizada) ||
                (usuario.TipoDeUsuario ?? string.Empty).ToUpper().Contains(busquedaNormalizada) ||
                (usuario.NormalizedUserName ?? string.Empty).ToUpper().Contains(busquedaNormalizada) ||
                (usuario.IdPersona ?? string.Empty).ToUpper().Contains(busquedaNormalizada) ||
                (usuario.NombreADesplegar ?? string.Empty).ToUpper().Contains(busquedaNormalizada) ||
                (usuario.EstadoDeUsuario ?? string.Empty).ToUpper().Contains(busquedaNormalizada) ||
                (usuario.InformacionAdicional ?? string.Empty).ToUpper().Contains(busquedaNormalizada) ||
                (incluyeActivos && usuario.Activo == true) ||
                (incluyeInactivos && usuario.Activo != true));
        }

        var total = await query.LongCountAsync(cancellationToken);

        var usuarios = await query
            .OrderBy(usuario => usuario.NombreADesplegar ?? usuario.Email)
            .ThenBy(usuario => usuario.Email)
            .Skip((numeroDePagina - 1) * cantidadPorPagina)
            .Take(cantidadPorPagina)
            .ToListAsync(cancellationToken);

        var items = usuarios
            .Select(MapUsuario)
            .ToArray();

        CommandResponse.Data = new BuscarUsuariosPaginadosResponse(
            numeroDePagina,
            cantidadPorPagina,
            total,
            items);
        CommandResponse.Result = true;
        return CommandResponse;
    }

    private static UsuarioPaginadoItemResponse MapUsuario(Usuario usuario)
    {
        var informacion = usuario.InformacionAdicional.ToInformacionAdicionalModel();

        return new UsuarioPaginadoItemResponse(
            usuario.Id,
            usuario.PhoneNumber,
            usuario.Descripcion,
            informacion.Nacionalidad,
            informacion.DocumentoDeIdentidad,
            informacion.NumeroDeDocumento,
            informacion.CodigoValidadorDocumento,
            informacion.PrimerNombre,
            informacion.SegundoNombre,
            informacion.PrimerApellido,
            informacion.SegundoApellido,
            informacion.SexoDeclarativo,
            informacion.SexoRegistral,
            informacion.FechaDeNacimiento,
            usuario.Email,
            usuario.TipoDeUsuario,
            usuario.NormalizedUserName,
            usuario.EmailConfirmed,
            usuario.PhoneNumberConfirmed,
            usuario.TwoFactorEnabled,
            usuario.IdPersona,
            usuario.NombreADesplegar,
            usuario.Activo,
            usuario.UsuarioBase,
            usuario.RequiereValidacionEnrrolamiento,
            usuario.EstadoDeUsuario);
    }

    private static bool MatchesBooleanSearch(string value, bool activo)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        return activo
            ? value is "TRUE" or "VERDADERO" or "ACTIVO" or "SI"
            : value is "FALSE" or "FALSO" or "INACTIVO" or "NO";
    }
}
