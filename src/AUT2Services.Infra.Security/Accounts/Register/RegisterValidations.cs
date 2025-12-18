using AUT2Services.Domain.Core.CommonValidators.Validators;
using AUT2Services.Domain.Enumerations;
using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.Register;
public abstract class RegisterValidations<T> : AbstractValidator<T> where T : RegisterCommand
{
    protected void Validate_CorreoElectronico()
    {
        RuleFor(rf => rf.CorreoElectronico)
            .NotEmpty()
                .WithMessage("El valor ingresado para el campo CorreoElectronico no puede estar vacio")
            .EmailAddress()
                .WithMessage("El correo electronico ingresado no es valido");
    }

    protected void Validate_NumeroDeTelefono()
    {
        RuleFor(rf => rf.NumeroDeTelefono)
            .NotEmpty()
                .WithMessage("El valor ingresado no puede estar vacio")
            .Length(8, 100)
                .WithMessage("El valor ingresado debe contener entre 8 y 100 caracteres");
    }

    protected void Validate_TipoDeUsuario()
    {
        RuleFor(rf => rf.TipoDeUsuario)
            .Must((x, y) => CommonValidator.EnumerationValidator(typeof(EnumTipoDeUsuario), x.TipoDeUsuario))
                .WithMessage("El valor ingresado para el campo TipoDeUsuario debe ser un valor valido definido en la enumeracion")
            .Length(3, 100)
                .WithMessage("El valor ingresado debe contener entre 3 y 100 caracteres");
    }

    protected void Validate_Password()
    {
        RuleFor(rf => rf.TipoDeUsuario)
            .NotEmpty()
                .WithMessage("El valor ingresado no puede estar vacio");
    }

    protected void Validate_NumeroDeDocumento()
    {
        RuleFor(rf => rf.NumeroDeDocumento)
            .NotEmpty()
                .WithMessage("El valor ingresado no puede estar vacio")
            .Length(5, 100)
                .WithMessage("El valor ingresado debe contener entre 5 y 100 caracteres");
    }

}