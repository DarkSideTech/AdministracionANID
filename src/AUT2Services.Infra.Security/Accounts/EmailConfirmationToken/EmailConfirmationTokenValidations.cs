using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.EmailConfirmationToken;

public abstract class EmailConfirmationTokenValidations<T> : AbstractValidator<T> where T : EmailConfirmationTokenCommand
{
    protected void Validate_UserId()
    {
        RuleFor(rf => rf.UserId)
            .NotEmpty()
                .WithMessage("El valor ingresado para el campo CorreoElectronico no puede estar vacio");
    }

    protected void Validate_Token()
    {
        RuleFor(rf => rf.Token)
            .NotEmpty()
                .WithMessage("El valor ingresado no puede estar vacio");
    }

}
