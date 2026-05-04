using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.Roles;

public class BuscarRolesCommandValidations : AbstractValidator<BuscarRolesCommand>
{
    private static readonly string[] AllowedStates = ["ACTIVOS", "INACTIVOS", "TODOS"];

    public BuscarRolesCommandValidations()
    {
        RuleFor(command => command.Estado)
            .Must(value => AllowedStates.Contains(NormalizeState(value), StringComparer.Ordinal))
            .WithMessage("Estado debe ser ACTIVOS, INACTIVOS o TODOS.");
    }

    private static string NormalizeState(string? value)
    {
        return (value ?? "ACTIVOS").Trim().ToUpperInvariant();
    }
}
