using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.ModificaCorreoElectronico;

public abstract class ModificaCorreoElectronicoValidations<T> : AbstractValidator<T> where T : ModificaCorreoElectronicoCommand
{
    protected void Validate_IdUsuario()
    {
        RuleFor(rf => rf.IdUsuario)
            .NotEmpty()
                .WithMessage("El valor ingresado para el campo IdUsuario no puede estar vacio");
    }

    protected void Validate_NuevoCorreoElectronico()
    {
        RuleFor(rf => rf.NuevoCorreoElectronico)
            .NotEmpty()
                .WithMessage("El valor ingresado para el campo NuevoCorreoElectronico no puede estar vacio")
            .EmailAddress()
                .WithMessage("El correo electronico ingresado no es valido");
    }
}
