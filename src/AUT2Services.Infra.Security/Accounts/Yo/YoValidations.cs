using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.Yo;

public abstract class YoValidations<T> : AbstractValidator<T> where T : YoCommand
{
    protected void Validate_Yo()
    {
    }
}