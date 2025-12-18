using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.ValidateEmail;

public abstract class EmailConfirmationTokenValidations<T> : AbstractValidator<T> where T : EmailConfirmationTokenCommand
{
    protected void Validate_UserId()
    {
        RuleFor(rf => rf.UserId)
            .NotEmpty()
                .WithMessage("El valor ingresado no puede estar vacio");
    }

    protected void Validate_ConfirmationToken()
    {
        RuleFor(rf => rf.ConfirmationToken)
            .NotEmpty()
                .WithMessage("El valor ingresado no puede estar vacio");
    }

}
