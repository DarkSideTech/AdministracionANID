namespace AUT2Services.Infra.Security.Records;

public sealed record RegisterResponse(
    string Email,
    bool RequiresEmailConfirmation,
    bool CanResendConfirmationEmail,
    string Message,
    string? ConfirmationUrl,
    string? ValidationToken
);
