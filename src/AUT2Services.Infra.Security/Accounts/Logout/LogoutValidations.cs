using FluentValidation;

namespace AUT2Services.Infra.Security.Accounts.Logout;

public abstract class LogoutValidations<T> : AbstractValidator<T> where T : LogoutCommand
{ 
}
