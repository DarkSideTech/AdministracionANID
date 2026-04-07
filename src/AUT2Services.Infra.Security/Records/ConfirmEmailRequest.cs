namespace AUT2Services.Infra.Security.Records;

public sealed record ConfirmEmailRequest(
    string UserId,
    string Token
);
