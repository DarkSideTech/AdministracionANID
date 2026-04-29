using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.Roles;

public class ModificaRolCommandValidations : AbstractValidator<ModificaRolCommand>
{
    public ModificaRolCommandValidations()
    {
        RuleFor(command => command.IdRol)
            .NotEmpty()
            .WithMessage("El valor ingresado para el campo IdRol no puede estar vacio");

        RuleFor(command => command.Descripcion)
            .MaximumLength(1000)
            .WithMessage("Descripcion no puede superar los 1000 caracteres.");
    }
}
