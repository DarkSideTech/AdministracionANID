using AUT2Services.Domain.Core.CommonValidators.Validators;
using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.ConfirmaRecuperacionClave;

public abstract class ConfirmaRecuperacionClaveValidations<T> : AbstractValidator<T> where T : ConfirmaRecuperacionClaveCommand
{
    protected void Validate_CorreoElectronico()
    {
        RuleFor(rf => rf.CorreoElectronico)
            .NotEmpty()
                .WithMessage("Debes ingresar el correo electronico.")
            .EmailAddress()
                .WithMessage("El correo electronico ingresado no es valido.");
    }

    protected void Validate_CodigoValidacion()
    {
        RuleFor(rf => rf.CodigoValidacion)
            .NotEmpty()
                .WithMessage("Debes ingresar el codigo de validacion enviado al correo electronico.")
            .MaximumLength(20)
                .WithMessage("El codigo de validacion ingresado no es valido.");
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
