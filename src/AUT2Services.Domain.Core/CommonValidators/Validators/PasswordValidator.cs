using FluentValidation;

namespace AUT2Services.Domain.Core.CommonValidators.Validators;

public static partial class CommonValidator
{
    public static IRuleBuilder<T, string> PasswordValidator<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength = 8, int maximunLength = 16)
    {
        var options = ruleBuilder
            .NotEmpty()
            .Length(minimumLength, maximunLength)
                .WithMessage($"La clave de acceso debe contener entre {minimumLength} y {maximunLength} caracteres")
            .Matches(@"[A-Z]+")
                .WithMessage("La clave de acceso debe contener como minimo una letra en mayusculas")
            .Matches(@"[a-z]+")
                .WithMessage("La clave de acceso debe contener como minimo una letra en minusculas")
            .Matches(@"[0-9]+")
                .WithMessage("La clave de acceso debe contener como minimo 1 numero")
            .Matches(@"[\!\?\*\.\@\%\$]+")
                .WithMessage("La clave de acceso debe contener como minimo uno de los siguientes caracteres (!?*.@%$)");

        return options;
    }
}
