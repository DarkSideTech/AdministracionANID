using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.ValidateEmail;

public abstract class EmailConfirmationTokenValidations<T> : AbstractValidator<T> where T : EmailConfirmationTokenCommand
{
    protected void Validate_Email()
    {
        RuleFor(rf => rf.Email)
            .NotEmpty()
                .WithMessage("El valor ingresado para el campo CorreoElectronico no puede estar vacio")
            .EmailAddress()
                .WithMessage("El correo electronico ingresado no es valido");
    }

    protected void Validate_ConfirmationToken()
    {
        RuleFor(rf => rf.ConfirmationToken)
            .NotEmpty()
                .WithMessage("El valor ingresado no puede estar vacio");
    }

}
