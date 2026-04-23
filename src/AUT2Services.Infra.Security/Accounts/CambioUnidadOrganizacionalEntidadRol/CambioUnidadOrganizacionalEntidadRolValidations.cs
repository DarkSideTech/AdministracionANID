using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.CambioUnidadOrganizacionalEntidadRol;

public abstract class CambioUnidadOrganizacionalEntidadRolValidations<T> : AbstractValidator<T> where T : CambioUnidadOrganizacionalEntidadRolCommand
{
    protected const string InvalidSelectionMessage = "La unidad Organizacion y Rol seleccionado no permite cambiar la configuracion, contactarse con el Administrador";

    protected void Validate_Id_Entidad()
    {
        RuleFor(command => command.Id_Entidad)
            .NotEmpty()
                .WithMessage(InvalidSelectionMessage)
            .Must(value => Guid.TryParse(value, out _))
                .WithMessage(InvalidSelectionMessage);
    }

    protected void Validate_Id_Rol()
    {
        RuleFor(command => command.Id_Rol)
            .NotEmpty()
                .WithMessage(InvalidSelectionMessage)
            .Must(value => Guid.TryParse(value, out _))
                .WithMessage(InvalidSelectionMessage);
    }
}
