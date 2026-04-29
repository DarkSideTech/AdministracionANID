using AUT2Services.Domain.Core.Commands;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Accounts.Roles;

public abstract class RolIdCommand : Command
{
    public string? IdRol { get; set; } = string.Empty;
    public required HttpRequest Request { get; set; }
    public required HttpResponse Response { get; set; }
}

public sealed class RolIdCommandValidations<TCommand> : AbstractValidator<TCommand>
    where TCommand : RolIdCommand
{
    public RolIdCommandValidations()
    {
        RuleFor(command => command.IdRol)
            .NotEmpty()
            .WithMessage("El valor ingresado para el campo IdRol no puede estar vacio");
    }
}
