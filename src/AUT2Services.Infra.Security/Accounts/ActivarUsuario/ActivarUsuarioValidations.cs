using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.ActivarUsuario;

public abstract class ActivarUsuarioValidations<T> : AbstractValidator<T> where T : ActivarUsuarioCommand
{
    protected void Validate_IdUsuario()
    {
        RuleFor(rf => rf.IdUsuario)
            .NotEmpty()
            .WithMessage("El valor ingresado para el campo IdUsuario no puede estar vacio");
    }
}
