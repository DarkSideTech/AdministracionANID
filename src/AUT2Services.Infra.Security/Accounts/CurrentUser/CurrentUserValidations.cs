using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.CurrentUser;

public abstract class CurrentUserValidations<T> : AbstractValidator<T> where T : CurrentUserCommand
{
    protected void Validate_CurrentUser()
    {
    }
}