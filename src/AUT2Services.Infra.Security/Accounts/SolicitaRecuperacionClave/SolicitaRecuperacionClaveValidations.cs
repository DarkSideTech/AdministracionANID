using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.SolicitaRecuperacionClave;

public abstract class SolicitaRecuperacionClaveValidations<T> : AbstractValidator<T> where T : SolicitaRecuperacionClaveCommand
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
