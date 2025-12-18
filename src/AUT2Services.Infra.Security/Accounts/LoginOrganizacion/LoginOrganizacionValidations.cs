using AUT2Services.Domain.Core.CommonValidators.Validators;
using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.LoginOrganizacion;

public abstract class LoginOrganizacionValidations<T> : AbstractValidator<T> where T : LoginOrganizacionCommand
{
    protected void Validate_UserName()
    {
        RuleFor(rf => rf.Email)
            .NotEmpty()
                .WithMessage("El valor ingresado no puede estar vacio")
            .Length(3, 100)
                .WithMessage("El valor ingresado debe contener entre 3 y 100 caracteres");
    }

    protected void Validate_Password()
    {
        RuleFor(rf => rf.Password)
            .NotNull()
            .NotEmpty()
                .WithMessage("El valor ingresado no puede estar vacio")
            .PasswordValidator(8, 12);
    }

    protected void Validate_Organizacion()
    {
        RuleFor(rf => rf.Organizacion)
            .NotEmpty()
                .WithMessage("El valor ingresado no puede estar vacio")
            .Length(3, 100)
                .WithMessage("El valor ingresado debe contener entre 3 y 100 caracteres");
    }
}

