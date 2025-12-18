using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.RefreshToken;

public abstract class RefreshTokenValidations<T> : AbstractValidator<T> where T : RefreshTokenCommand
{
    protected void Validate_RefreshToken()
    {
        RuleFor(rf => rf.RefreshToken)
            .NotEmpty()
                .WithMessage("El valor ingresado no puede estar vacio");
    }
}