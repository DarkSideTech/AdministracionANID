using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.ResendEmailConfirmationToken;

public abstract class ResendEmailConfirmationTokenValidations<T> : AbstractValidator<T> where T : ResendEmailConfirmationTokenCommand
{
    protected void Validate_Email()
    {
        RuleFor(rf => rf.Email)
            .NotEmpty()
                .WithMessage("El valor ingresado para el campo CorreoElectronico no puede estar vacio");
    }
}
