using AUT2Services.Domain.Core.CommonValidators.Validators;
using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.SolicitaCambioClave;

public abstract class SolicitaCambioClaveValidations<T> : AbstractValidator<T> where T : SolicitaCambioClaveCommand
{
    protected void Validate_IdUsuario()
    {
        RuleFor(rf => rf.IdUsuario)
            .NotEmpty()
            .WithMessage("El valor ingresado para el campo IdUsuario no puede estar vacio");
    }

    protected void Validate_ClaveActual()
    {
        RuleFor(rf => rf.ClaveActual)
            .NotEmpty()
            .WithMessage("Debes ingresar la clave anterior");
    }

    protected void Validate_NuevaClave()
    {
        RuleFor(rf => rf.NuevaClave)
            .PasswordValidator(8, 12);
    }

    protected void Validate_ConfirmaNuevaClave()
    {
        RuleFor(rf => rf.ConfirmaNuevaClave)
            .NotEmpty()
                .WithMessage("Debes reingresar la nueva clave")
            .Equal(rf => rf.NuevaClave)
                .WithMessage("La confirmacion de la nueva clave no corresponde");
    }
}
