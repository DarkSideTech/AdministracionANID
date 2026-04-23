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

    protected void Validate_State()
    {
        RuleFor(rf => rf.State)
            .NotEmpty()
                .WithMessage("El token de estado no puede estar vacio.")
            .Length(8, 256)
                .WithMessage("El token de estado no tiene un largo valido.");
    }
}
