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
            .Length(8, 100)
                .WithMessage("El valor ingresado debe contener entre 8 y 100 caracteres")
            .When(rf => !string.IsNullOrWhiteSpace(rf.NumeroDeTelefono));
    }

    protected void Validate_TipoDeUsuario()
    {
        RuleFor(rf => rf.TipoDeUsuario)
            .Must((_, y) => CommonValidator.EnumerationValidator(typeof(EnumTipoDeUsuario), y))
                .WithMessage("El valor ingresado para el campo TipoDeUsuario debe ser un valor valido definido en la enumeracion")
            .Length(3, 100)
                .WithMessage("El valor ingresado debe contener entre 3 y 100 caracteres");
    }

    protected void Validate_Password()
    {
        RuleFor(rf => rf.Contraseña)
            .PasswordValidator(8, 12);
    }

    protected void Validate_ConfirmaContraseña()
    {
        RuleFor(rf => rf.ConfirmaContraseña)
            .NotEmpty()
                .WithMessage("Debes confirmar la contraseña")
            .Equal(rf => rf.Contraseña)
                .WithMessage("La confirmacion de la contraseña no corresponde");
    }

    protected void Validate_NumeroDeDocumento()
    {
        RuleFor(rf => rf.NumeroDeDocumento)
            .NotEmpty()
                .WithMessage("El valor ingresado no puede estar vacio")
            .Length(5, 100)
                .WithMessage("El valor ingresado debe contener entre 5 y 100 caracteres");
    }

    protected void Validate_Nacionalidad()
    {
        RuleFor(rf => rf.Nacionalidad)
            .NotEmpty()
                .WithMessage("El valor ingresado para el campo Nacionalidad no puede estar vacio")
            .Must((_, y) => CommonValidator.EnumerationValidator(typeof(EnumNacionalidad), y))
                .WithMessage("El valor ingresado para el campo Nacionalidad debe ser un valor valido definido en la enumeracion");
    }

    protected void Validate_DocumentoDeIdentidad()
    {
        RuleFor(rf => rf.DocumentoDeIdentidad)
            .NotEmpty()
                .WithMessage("El valor ingresado para el campo DocumentoDeIdentidad no puede estar vacio")
            .Must((_, y) => CommonValidator.EnumerationValidator(typeof(EnumDocumentoDeIdentidad), y))
                .WithMessage("El valor ingresado para el campo DocumentoDeIdentidad debe ser un valor valido definido en la enumeracion");
    }

    protected void Validate_CodigoValidadorDocumento()
    {
        RuleFor(rf => rf.CodigoValidadorDocumento)
            .NotEmpty()
                .WithMessage("El valor ingresado para el campo CodigoValidadorDocumento no puede estar vacio")
            .Length(1, 100)
                .WithMessage("El valor ingresado debe contener entre 1 y 100 caracteres");
    }

    protected void Validate_PrimerNombre()
    {
        RuleFor(rf => rf.PrimerNombre)
            .NotEmpty()
                .WithMessage("El valor ingresado para el campo PrimerNombre no puede estar vacio")
            .Length(2, 100)
                .WithMessage("El valor ingresado debe contener entre 2 y 100 caracteres");
    }

    protected void Validate_PrimerApellido()
    {
        RuleFor(rf => rf.PrimerApellido)
            .NotEmpty()
                .WithMessage("El valor ingresado para el campo PrimerApellido no puede estar vacio")
            .Length(2, 100)
                .WithMessage("El valor ingresado debe contener entre 2 y 100 caracteres");
    }

    protected void Validate_SexoDeclarativo()
    {
        RuleFor(rf => rf.SexoDeclarativo)
            .NotEmpty()
                .WithMessage("El valor ingresado para el campo SexoDeclarativo no puede estar vacio")
            .Must((_, y) => CommonValidator.EnumerationValidator(typeof(EnumSexoDeclarativo), y))
                .WithMessage("El valor ingresado para el campo SexoDeclarativo debe ser un valor valido definido en la enumeracion");
    }

    protected void Validate_SexoRegistral()
    {
        RuleFor(rf => rf.SexoRegistral)
            .NotEmpty()
                .WithMessage("El valor ingresado para el campo SexoRegistral no puede estar vacio")
            .Must((_, y) => CommonValidator.EnumerationValidator(typeof(EnumSexoRegistral), y))
                .WithMessage("El valor ingresado para el campo SexoRegistral debe ser un valor valido definido en la enumeracion");
    }

    protected void Validate_FechaDeNacimiento()
    {
        RuleFor(rf => rf.FechaDeNacimiento)
            .NotEmpty()
                .WithMessage("El valor ingresado para el campo FechaDeNacimiento no puede estar vacio");
    }

    protected void Validate_TerminosYCondiciones()
    {
        RuleFor(rf => rf.TerminosYCondiciones)
            .Equal(true)
                .WithMessage("Debes aceptar los terminos y condiciones");
    }

}
