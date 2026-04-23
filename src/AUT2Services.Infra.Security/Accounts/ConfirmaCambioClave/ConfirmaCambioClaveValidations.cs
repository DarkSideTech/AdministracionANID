using AUT2Services.Domain.Core.CommonValidators.Validators;
using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.ConfirmaCambioClave;

public abstract class ConfirmaCambioClaveValidations<T> : AbstractValidator<T> where T : ConfirmaCambioClaveCommand
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

    protected void Validate_CodigoValidacion()
    {
        RuleFor(rf => rf.CodigoValidacion)
            .NotEmpty()
            .WithMessage("Debes ingresar el codigo de validacion enviado al correo electronico.");
    }
}
