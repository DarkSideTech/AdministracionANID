using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.DesactivarUsuario;

public abstract class DesactivarUsuarioValidations<T> : AbstractValidator<T> where T : DesactivarUsuarioCommand
{
    protected void Validate_IdUsuario()
    {
        RuleFor(rf => rf.IdUsuario)
            .NotEmpty()
            .WithMessage("El valor ingresado para el campo IdUsuario no puede estar vacio");
    }
}
