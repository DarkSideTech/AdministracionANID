using AUT2Services.Domain.Core.CommonValidators.Validators;
using AUT2Services.Domain.Enumerations;
using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.ModificaUsuario;
public abstract class ModificaUsuarioValidations<T> : AbstractValidator<T> where T : ModificaUsuarioCommand
{
    protected void Validate_IdUsuario()
    {
        RuleFor(rf => rf.IdUsuario)
            .NotEmpty()
                .WithMessage("El valor ingresado para el campo IdUsuario no puede estar vacio");
    }

    protected void Validate_CorreoElectronico()
    {
        RuleFor(rf => rf.CorreoElectronico)
            .EmailAddress()
                .WithMessage("El correo electronico ingresado no es valido")
            .When(rf => !string.IsNullOrWhiteSpace(rf.CorreoElectronico));
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
                .WithMessage("El valor ingresado debe contener entre 3 y 100 caracteres")
            .When(rf => !string.IsNullOrWhiteSpace(rf.TipoDeUsuario));
    }

    protected void Validate_Password()
    {
        RuleFor(rf => rf.Contraseña)
            .PasswordValidator(8, 12)
            .When(rf => !string.IsNullOrWhiteSpace(rf.Contraseña));
    }

    protected void Validate_ConfirmaContraseña()
    {
        RuleFor(rf => rf.ConfirmaContraseña)
            .Equal(rf => rf.Contraseña)
                .WithMessage("La confirmacion de la contraseña no corresponde")
            .When(rf => !string.IsNullOrWhiteSpace(rf.Contraseña) || !string.IsNullOrWhiteSpace(rf.ConfirmaContraseña));
    }

    protected void Validate_NumeroDeDocumento()
    {
        RuleFor(rf => rf.NumeroDeDocumento)
            .Length(5, 100)
                .WithMessage("El valor ingresado debe contener entre 5 y 100 caracteres")
            .When(rf => !string.IsNullOrWhiteSpace(rf.NumeroDeDocumento));
    }

    protected void Validate_Nacionalidad()
    {
        RuleFor(rf => rf.Nacionalidad)
            .Must((_, y) => CommonValidator.EnumerationValidator(typeof(EnumNacionalidad), y))
                .WithMessage("El valor ingresado para el campo Nacionalidad debe ser un valor valido definido en la enumeracion")
            .When(rf => !string.IsNullOrWhiteSpace(rf.Nacionalidad));
    }

    protected void Validate_DocumentoDeIdentidad()
    {
        RuleFor(rf => rf.DocumentoDeIdentidad)
            .Must((_, y) => CommonValidator.EnumerationValidator(typeof(EnumDocumentoDeIdentidad), y))
                .WithMessage("El valor ingresado para el campo DocumentoDeIdentidad debe ser un valor valido definido en la enumeracion")
            .When(rf => !string.IsNullOrWhiteSpace(rf.DocumentoDeIdentidad));
    }

    protected void Validate_CodigoValidadorDocumento()
    {
        RuleFor(rf => rf.CodigoValidadorDocumento)
            .Length(1, 100)
                .WithMessage("El valor ingresado debe contener entre 1 y 100 caracteres")
            .When(rf => !string.IsNullOrWhiteSpace(rf.CodigoValidadorDocumento));
    }

    protected void Validate_PrimerNombre()
    {
        RuleFor(rf => rf.PrimerNombre)
            .Length(2, 100)
                .WithMessage("El valor ingresado debe contener entre 2 y 100 caracteres")
            .When(rf => !string.IsNullOrWhiteSpace(rf.PrimerNombre));
    }

    protected void Validate_PrimerApellido()
    {
        RuleFor(rf => rf.PrimerApellido)
            .Length(2, 100)
                .WithMessage("El valor ingresado debe contener entre 2 y 100 caracteres")
            .When(rf => !string.IsNullOrWhiteSpace(rf.PrimerApellido));
    }

    protected void Validate_SexoDeclarativo()
    {
        RuleFor(rf => rf.SexoDeclarativo)
            .Must((_, y) => CommonValidator.EnumerationValidator(typeof(EnumSexoDeclarativo), y))
                .WithMessage("El valor ingresado para el campo SexoDeclarativo debe ser un valor valido definido en la enumeracion")
            .When(rf => !string.IsNullOrWhiteSpace(rf.SexoDeclarativo));
    }

    protected void Validate_SexoRegistral()
    {
        RuleFor(rf => rf.SexoRegistral)
            .Must((_, y) => CommonValidator.EnumerationValidator(typeof(EnumSexoRegistral), y))
                .WithMessage("El valor ingresado para el campo SexoRegistral debe ser un valor valido definido en la enumeracion")
            .When(rf => !string.IsNullOrWhiteSpace(rf.SexoRegistral));
    }

    protected void Validate_FechaDeNacimiento()
    {
        RuleFor(rf => rf.FechaDeNacimiento)
            .NotEmpty()
                .WithMessage("El valor ingresado para el campo FechaDeNacimiento no puede estar vacio")
            .When(rf => rf.FechaDeNacimiento.HasValue);
    }

    protected void Validate_TerminosYCondiciones()
    {
        RuleFor(rf => rf.TerminosYCondiciones)
            .Equal(true)
                .WithMessage("Debes aceptar los terminos y condiciones")
            .When(rf => rf.TerminosYCondiciones.HasValue);
    }

}
