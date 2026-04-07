namespace AUT2Services.Infra.Security.Records;

public sealed record AccessTokenResult(
    string Token,
    DateTime ExpiresAtUtc
);