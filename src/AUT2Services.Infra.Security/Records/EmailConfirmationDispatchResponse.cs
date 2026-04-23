namespace AUT2Services.Infra.Security.Records;

public sealed record EmailConfirmationDispatchResponse(
    string Email,
    string Message,
    string? ConfirmationUrl,
    string? ValidationToken
);
