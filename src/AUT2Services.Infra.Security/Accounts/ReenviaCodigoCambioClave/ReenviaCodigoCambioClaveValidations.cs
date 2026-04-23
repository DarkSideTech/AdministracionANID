using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.ReenviaCodigoCambioClave;

public abstract class ReenviaCodigoCambioClaveValidations<T> : AbstractValidator<T> where T : ReenviaCodigoCambioClaveCommand
{
    protected void Validate_IdUsuario()
    {
        RuleFor(rf => rf.IdUsuario)
            .NotEmpty()
            .WithMessage("El valor ingresado para el campo IdUsuario no puede estar vacio");
    }
}
