using AUT2Services.Domain.Core.CommonValidators.Validators;
using AUT2Services.Domain.Enumerations;
using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.BaseEntity;

public abstract class BaseEntityValidations<T> : AbstractValidator<T> where T : BaseEntityCommand
{
    protected void Validate_CodigoOrganizacion()
    {
        RuleFor(rf => rf.CodigoOrganizacion)
            .NotEmpty()
                .WithMessage("El valor ingresado para el campo Id no puede estar vacio")
            .Length(3, 100)
                .WithMessage("El valor ingresado debe contener entre 3 y 100 caracteres");
    }

    protected void Validate_NombreOrganizacion()
    {
        RuleFor(rf => rf.NombreOrganizacion)
            .NotEmpty()
                .WithMessage("El valor ingresado para el campo Nombre no puede estar vacio")
            .Length(3, 255)
                .WithMessage("El valor ingresado debe contener entre 3 y 255 caracteres");
    }

    protected void Validate_Id_Usuario()
    {
        RuleFor(rf => rf.Id_Usuario)
            .NotEqual(Guid.Empty)
            .NotEmpty()
                .WithMessage("El valor ingresado para el campo Id no puede estar vacio");
    }

    protected void Validate_TipoDeEntidad()
    {
        RuleFor(rf => rf.TipoDeEntidad)
            .Must((_, y) => CommonValidator.EnumerationValidator(typeof(EnumTipoDeEntidad), y))
                .WithMessage("El valor ingresado para el campo TipoDeEntidad debe ser un valor valido definido en la enumeracion")
            .Length(3, 100)
                .WithMessage("El valor ingresado debe contener entre 3 y 100 caracteres");
    }

    protected void Validate_CorreoElectronico()
    {
        RuleFor(rf => rf.CorreoElectronico)
            .NotEmpty()
                .WithMessage("El valor ingresado para el campo CorreoElectronico no puede estar vacio")
            .EmailAddress()
                .WithMessage("El correo electronico ingresado no es valido")
            .When(rf => !rf.PermitirCorreoElectronicoVacio);

        RuleFor(rf => rf.CorreoElectronico)
            .EmailAddress()
                .WithMessage("El correo electronico ingresado no es valido")
            .When(rf => rf.PermitirCorreoElectronicoVacio && !string.IsNullOrWhiteSpace(rf.CorreoElectronico));
    }
}
