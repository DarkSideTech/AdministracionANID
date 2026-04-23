using AUT2Services.Domain.Core.Models;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Records;

namespace AUT2Services.Infra.Security.Interfaces;

public interface IEmailConfirmationMessageService
{
    Task<EmailConfirmationDispatch> CreateDispatchAsync(Usuario usuario);
    EmailDataModel BuildEmailMessage(Usuario usuario, EmailConfirmationDispatch dispatch);
    string? GetValidationTokenForResponse(EmailConfirmationDispatch dispatch);
}
