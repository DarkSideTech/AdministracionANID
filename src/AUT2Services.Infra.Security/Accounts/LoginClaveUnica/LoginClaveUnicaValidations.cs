using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.LoginClaveUnica;

public abstract class LoginClaveUnicaValidations<T> : AbstractValidator<T> where T : LoginClaveUnicaCommand
{
    protected void Validate_Code()
    {
        RuleFor(rf => rf.Code)
            .NotEmpty()
                .WithMessage("El codigo de autorizacion no puede estar vacio.")
            .Length(8, 256)
                .WithMessage("El codigo de autorizacion no tiene un largo valido.");
    }
}
