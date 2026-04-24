using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.BuscarUsuariosPaginados;

public abstract class BuscarUsuariosPaginadosValidations<T> : AbstractValidator<T> where T : BuscarUsuariosPaginadosCommand
{
    protected void Validate_NumeroDePagina()
    {
        RuleFor(rf => rf.NumeroDePagina)
            .GreaterThanOrEqualTo(1)
            .WithMessage("El valor ingresado para NumeroDePagina debe ser mayor o igual a 1")
            .When(rf => rf.NumeroDePagina.HasValue);
    }

    protected void Validate_CantidadPorPagina()
    {
        RuleFor(rf => rf.CantidadPorPagina)
            .InclusiveBetween(1, 100)
            .WithMessage("El valor ingresado para CantidadPorPagina debe estar entre 1 y 100")
            .When(rf => rf.CantidadPorPagina.HasValue);
    }

    protected void Validate_Busqueda()
    {
        RuleFor(rf => rf.Busqueda)
            .MaximumLength(200)
            .WithMessage("El valor ingresado para Busqueda no puede exceder 200 caracteres")
            .When(rf => !string.IsNullOrWhiteSpace(rf.Busqueda));
    }
}
