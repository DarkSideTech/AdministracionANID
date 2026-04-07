namespace AUT2Services.Infra.Security.Records;

public sealed record ResendEmailConfirmationTokenRequest(
    string Email
);