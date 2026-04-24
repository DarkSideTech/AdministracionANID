using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.ReenviaCodigoRecuperacionClave;

public abstract class ReenviaCodigoRecuperacionClaveValidations<T> : AbstractValidator<T> where T : ReenviaCodigoRecuperacionClaveCommand
{
    protected void Validate_CorreoElectronico()
    {
        RuleFor(rf => rf.CorreoElectronico)
            .NotEmpty()
                .WithMessage("Debes ingresar el correo electronico.")
            .EmailAddress()
                .WithMessage("El correo electronico ingresado no es valido.");
    }
}
