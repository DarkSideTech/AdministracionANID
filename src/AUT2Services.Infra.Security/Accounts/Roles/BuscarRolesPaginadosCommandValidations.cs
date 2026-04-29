using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.Roles;

public class BuscarRolesPaginadosCommandValidations : AbstractValidator<BuscarRolesPaginadosCommand>
{
    public BuscarRolesPaginadosCommandValidations()
    {
        RuleFor(command => command.NumeroDePagina)
            .GreaterThan(0)
            .WithMessage("NumeroDePagina debe ser mayor a cero.");

        RuleFor(command => command.CantidadPorPagina)
            .InclusiveBetween(1, 100)
            .WithMessage("CantidadPorPagina debe estar entre 1 y 100.");

        RuleFor(command => command.Busqueda)
            .MaximumLength(200)
            .WithMessage("Busqueda no puede superar los 200 caracteres.");
    }
}
