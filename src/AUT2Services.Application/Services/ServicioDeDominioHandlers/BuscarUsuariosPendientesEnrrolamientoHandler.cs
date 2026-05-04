using AUT2Services.Application.ViewModels.ServiciosDeDominio;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Infra.Security.Models;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Application.Services.ServicioDeDominioHandlers;

public partial class ServicioDeDominioServiceApp
{
    public async Task<CommandResponse> BuscarUsuariosPendientesEnrrolamiento(BuscarUsuariosPendientesEnrrolamientoServicioDeDominioViewModel command)
    {
        var result = new CommandResponse
        {
            Result = false
        };
        result.ValidationResult.Errors = [];

        if (!Guid.TryParse(userAccessor.GetIdUsuario(), out var idUsuarioActual))
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(BuscarUsuariosPendientesEnrrolamiento), "No fue posible determinar el usuario autenticado."));
            return result;
        }

        var rolValidaEnrrolamiento = await roleManager.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == EnumRolesBase.VALIDA_ENRROLAMIENTO);

        if (rolValidaEnrrolamiento is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(BuscarUsuariosPendientesEnrrolamiento), "No existe el rol VALIDA_ENRROLAMIENTO."));
            return result;
        }

        var usuarioValidaEnrrolamiento = await servicioDeDominioRepository.UsuarioConRolValidaEnrrolamiento(
            idUsuarioActual,
            Guid.Parse(rolValidaEnrrolamiento.Id));

        if (!usuarioValidaEnrrolamiento)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(BuscarUsuariosPendientesEnrrolamiento), "El usuario no tiene asignado el rol VALIDA_ENRROLAMIENTO."));
            return result;
        }

        var numeroDePagina = Math.Max(command.NumeroDePagina.GetValueOrDefault(1), 1);
        var cantidadPorPagina = Math.Clamp(command.CantidadPorPagina.GetValueOrDefault(10), 1, 100);
        var busqueda = command.Busqueda?.Trim();

        var query = userManager.Users
            .AsNoTracking()
            .Where(usuario =>
                usuario.RequiereValidacionEnrrolamiento == true &&
                usuario.EstadoDeUsuario == EnumEstadoDeUsuario.PROCESO_REGISTRO &&
                usuario.TipoDeUsuario != EnumTipoDeUsuario.NACIONAL);

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var busquedaNormalizada = busqueda.ToUpperInvariant();
            query = query.Where(usuario =>
                (usuario.Id ?? string.Empty).ToUpper().Contains(busquedaNormalizada) ||
                (usuario.Email ?? string.Empty).ToUpper().Contains(busquedaNormalizada) ||
                (usuario.NombreADesplegar ?? string.Empty).ToUpper().Contains(busquedaNormalizada) ||
                (usuario.TipoDeUsuario ?? string.Empty).ToUpper().Contains(busquedaNormalizada) ||
                (usuario.EstadoDeUsuario ?? string.Empty).ToUpper().Contains(busquedaNormalizada) ||
                (usuario.InformacionAdicional ?? string.Empty).ToUpper().Contains(busquedaNormalizada));
        }

        var total = await query.LongCountAsync();
        var usuarios = await query
            .OrderBy(usuario => usuario.NombreADesplegar ?? usuario.Email)
            .ThenBy(usuario => usuario.Email)
            .Skip((numeroDePagina - 1) * cantidadPorPagina)
            .Take(cantidadPorPagina)
            .ToListAsync();

        result.Data = new BuscarUsuariosPendientesEnrrolamientoResponse(
            numeroDePagina,
            cantidadPorPagina,
            total,
            usuarios.Select(MapUsuarioPendiente).ToArray());
        result.Result = true;
        return result;
    }

    private static UsuarioPendienteEnrrolamientoViewModel MapUsuarioPendiente(Domain.Security.Entities.Usuario usuario)
    {
        var informacion = usuario.InformacionAdicional.ToInformacionAdicionalModel();

        return new UsuarioPendienteEnrrolamientoViewModel(
            usuario.Id,
            usuario.Email,
            usuario.NombreADesplegar,
            usuario.TipoDeUsuario,
            usuario.EstadoDeUsuario,
            usuario.RequiereValidacionEnrrolamiento,
            usuario.EmailConfirmed,
            informacion.Nacionalidad,
            informacion.DocumentoDeIdentidad,
            informacion.NumeroDeDocumento,
            informacion.CodigoValidadorDocumento,
            informacion.PrimerNombre,
            informacion.SegundoNombre,
            informacion.PrimerApellido,
            informacion.SegundoApellido,
            informacion.FechaDeNacimiento);
    }
}
