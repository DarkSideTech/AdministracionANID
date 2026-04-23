namespace AUT2Services.Infra.Security.Records;

public sealed record PasswordChangeChallengeDispatchResponse(
    string Email,
    string Message,
    DateTimeOffset ExpiresAtUtc,
    DateTimeOffset CanResendAtUtc);
