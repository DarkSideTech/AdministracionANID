namespace AUT2Services.Infra.Security.Records;

public sealed record RegisterResponse(
    string Email,
    bool RequiresEmailConfirmation,
    string Message,
    string? ConfirmationUrl
);